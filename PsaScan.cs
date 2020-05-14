using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PopHistoryFunction.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PopHistoryFunction
{
    public class PsaScan
    {
        private PopHistoryFunctionContext _context { get; set; }

        public PsaScan(PopHistoryFunctionContext context)
        {
            _context = context;
        }

        [FunctionName("PsaScan")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            int setId = -1;

            try
            {
                var queryString = req.GetQueryParameterDictionary();
                string setIdKey = "setId";
                if (!queryString.ContainsKey(setIdKey) || !int.TryParse(queryString[setIdKey], out setId))
                {
                    return new BadRequestResult();
                }

                log.LogInformation("PsaScan Started");

                var web = new HtmlWeb();
                var psaSet = _context.PsaSet.FirstOrDefault(x => x.Id == setId);

                if (psaSet != null)
                {
                    var doc = web.Load($"https://www.psacard.com/Pop/GetItemTable?categoryID={psaSet.CategoryId}&headingID={psaSet.HeadingId}");

                    var ths = doc.DocumentNode.Descendants("TH").ToList();

                    var psaPopHistories = new List<PsaPopHistoryFunction>();

                    try
                    {
                        int cardNumber = FindColumnIndex(ths, "CARD NO.");
                        int name = FindColumnIndex(ths, "NAME");
                        int auth = FindColumnIndex(ths, "AUTH");
                        int pop01 = FindColumnIndex(ths, "1");
                        int pop02 = FindColumnIndex(ths, "2");
                        int pop03 = FindColumnIndex(ths, "3");
                        int pop04 = FindColumnIndex(ths, "4");
                        int pop05 = FindColumnIndex(ths, "5");
                        int pop06 = FindColumnIndex(ths, "6");
                        int pop07 = FindColumnIndex(ths, "7");
                        int pop08 = FindColumnIndex(ths, "8");
                        int pop085 = FindColumnIndex(ths, "8.5", false);
                        int pop09 = FindColumnIndex(ths, "9");
                        int pop095 = FindColumnIndex(ths, "9.5", false);
                        int pop10 = FindColumnIndex(ths, "10");

                        var trs = doc.DocumentNode.Descendants("TR").ToList();

                        foreach (var tr in trs)
                        {
                            var tds = tr.Descendants("TD").ToList();

                            if (!tds.Any() || string.IsNullOrWhiteSpace(tds[cardNumber].InnerText))
                            {
                                continue;
                            }

                            var names = tds[name]
                                .Descendants()
                                .Where(x =>
                                    x.Name.Equals("#text") &&
                                    !string.IsNullOrWhiteSpace(x.InnerText.Trim()) &&
                                    !x.InnerText.Trim().Equals(@"Shop", StringComparison.OrdinalIgnoreCase)
                                ).Select(x => x.InnerText.Trim());

                            PsaCard card = new PsaCard
                            {
                                CardNumber = tds[cardNumber].InnerText.Trim(),
                                NamePrimary = names.ElementAtOrDefault(0),
                                NameSecondary = names.ElementAtOrDefault(1),
                                SetId = psaSet.Id
                            };

                            var cardMatch = _context.PsaCard.FirstOrDefault(x =>
                                    x.CardNumber == card.CardNumber &&
                                    x.NamePrimary == card.NamePrimary &&
                                    x.NameSecondary == card.NameSecondary &&
                                    x.SetId == card.SetId);

                            if (cardMatch == null)
                            {
                                _context.PsaCard.Add(card);
                                _context.SaveChanges();
                            }

                            psaPopHistories.Add(new PsaPopHistoryFunction
                            {
                                CardId = cardMatch == null ? card.Id : cardMatch.Id,
                                PopAuth = FindPopulation(tds, auth),
                                Pop01 = FindPopulation(tds, pop01),
                                Pop02 = FindPopulation(tds, pop02),
                                Pop03 = FindPopulation(tds, pop03),
                                Pop04 = FindPopulation(tds, pop04),
                                Pop05 = FindPopulation(tds, pop05),
                                Pop06 = FindPopulation(tds, pop06),
                                Pop07 = FindPopulation(tds, pop07),
                                Pop08 = FindPopulation(tds, pop08),
                                Pop085 = FindPopulation(tds, pop085),
                                Pop09 = FindPopulation(tds, pop09),
                                Pop095 = FindPopulation(tds, pop095),
                                Pop10 = FindPopulation(tds, pop10)
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        log.LogError($"[{psaSet.CategoryId}] [{psaSet.HeadingId}] " + ex.Message);
                        throw;
                    }

                    _context.PsaPopHistoryFunction.AddRange(psaPopHistories);
                    _context.SaveChanges();
                }

                log.LogInformation("PsaScan Ended");
                return new OkObjectResult($"Ran with setId: {setId}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"PsaScan Caught Exception with setId: {setId}");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        private static int FindColumnIndex(List<HtmlNode> ths, string column, bool mandatory = true)
        {
            int index = ths.FindIndex(x => x.InnerText.Trim().Equals(column, StringComparison.OrdinalIgnoreCase));
            
            if (mandatory && index == -1)
            {
                throw new Exception($"Could not find column [{column.ToUpper()}]");
            }
            
            return index;
        }

        private static int? FindPopulation(List<HtmlNode> tds, int index)
        {
            if (index == -1)
            {
                return null;
            }

            var rows = tds[index]
                        .Descendants()
                        .Where(x =>
                            x.Name.Equals("#text") &&
                            !string.IsNullOrWhiteSpace(x.InnerText.Trim()))
                        .Select(x => x.InnerText.Trim());

            var population = rows.FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(population) && !population.Equals("-"))
            {
                return Convert.ToInt32(population);
            }
            else
            {
                return null;
            }
        }
    }
}
