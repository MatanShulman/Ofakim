using HtmlAgilityPack;
using Ofakim.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;
using Ofakim.Models;

namespace Ofakim.Services
{
    public class WriteFileService : IWriteToFile
    {
        string xmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Settings.xml");

        public void WriteToFile()
        {

            XmlSerializer xmlSerializerc = new XmlSerializer(typeof(List<BlockModel>));

            //download string and parse to BlockModel
            var downloadString = new TransformBlock<BlockModel, BlockModel>(async model =>
            {
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(model.Uri);
                var node = doc.GetElementbyId("siteHeader");
                var mainNode = node.NextSibling.SelectSingleNode("//div[@class=\"unit-rates___StyledDiv-sc-1dk593y-0 dEqdnx\"]");
                model.Value = mainNode.ChildNodes[0].InnerText;
                return model;
            });

            //validate that file exist and remove the old blockModel from list
            var xmlValidation = new TransformBlock<BlockModel, List<BlockModel>>(model =>
            {

                //if file exsit parse the list
                var blockList = new List<BlockModel>();
                if (File.Exists(xmlPath))
                {
                    try
                    {
                        using (var stream = File.OpenText(xmlPath))
                        {
                            blockList = (List<BlockModel>)xmlSerializerc.Deserialize(stream);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                //remove the old block
                var existBlock = blockList.Where(x => x.Type == model.Type).FirstOrDefault();
                if (existBlock != null)
                {
                    blockList.Remove(existBlock);
                }

                //add the current block
                blockList.Add(model);

                return blockList;
            });

            //override old XML
            var setXml = new ActionBlock<List<BlockModel>>(blockList =>
            {
                try
                {
                    using (var streamCreate = File.Create(xmlPath))
                    {
                        xmlSerializerc.Serialize(streamCreate, blockList);
                    }
                }
                catch
                {
                    throw;
                }
            });

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            downloadString.LinkTo(xmlValidation, linkOptions);
            xmlValidation.LinkTo(setXml, linkOptions);

            //post data
            downloadString.Post(new BlockModel { Uri = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=USD&To=ILS", Type = "USD/ISR" });
            downloadString.Post(new BlockModel { Uri = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=GBP&To=EUR", Type = "GBP/EUR" });
            downloadString.Post(new BlockModel { Uri = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=GBP&To=JPY", Type = "GBP/JPY" });
            downloadString.Post(new BlockModel { Uri = "https://www.xe.com/currencyconverter/convert/?Amount=1&From=GBP&To=EUR", Type = "EUR/USD" });
         

            //mark head of pipeline complete
            downloadString.Complete();

            // Wait for the last block in the pipeline 
            try
            {
                setXml.Completion.Wait();
            }
            catch
            {
                //log exception
                throw;
            }
        }
    }
}

