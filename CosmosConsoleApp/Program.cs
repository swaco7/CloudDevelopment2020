using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Diagnostics;
using Newtonsoft.Json;


namespace CosmosConsoleApp {
    class Program {
        public static async Task Main(string[] args) {
            var client = new DocumentClient(new Uri("https://clouddevelop2020rs.documents.azure.com:443/"), "ifc8BwJOZpcLDxbaAghqqGXyCxJ0A6aHBemEX5OSZqA2nKek9LlAVzz7k1jHl9McU3gMLamsh0bQ9D84ueBefQ=="); // set account and key
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("clouddevelop2020rs", "Container1"); // set database and collection

            for (int i = 1; i <= 10; i++) {
                var person = new Person { Id = i.ToString(), FirstName = "My", Surname = "Name", Category = i % 3 };

                Stopwatch stopwatch = Stopwatch.StartNew();
                await client.UpsertDocumentAsync(collectionUri, person);
                Console.WriteLine($"Inserted/Updated in {stopwatch.ElapsedMilliseconds} ms.");
            }

            var option = new FeedOptions { EnableCrossPartitionQuery = true };

            foreach (Document document in client.CreateDocumentQuery(
                    collectionUri,
                    new SqlQuerySpec("SELECT * FROM item WHERE item.category = @category",
                    new SqlParameterCollection(new[] { new SqlParameter { Name = "@category", Value = 1 } })),
                    option)) {
                Person person = (Person)(dynamic)document;

                Stopwatch stopwatch = Stopwatch.StartNew();
                await client.DeleteDocumentAsync(document.SelfLink, new RequestOptions { PartitionKey = new PartitionKey((object)person.Category ?? Undefined.Value) });
                Console.WriteLine($"Deleted in {stopwatch.ElapsedMilliseconds} ms.");
            }

            var query = client.CreateDocumentQuery<Person>(collectionUri, option)
                .Where(p => p.Category == 2);

            foreach (var person in query) {
                Stopwatch stopwatch = Stopwatch.StartNew();
                await client.DeleteDocumentAsync(person.Self, new RequestOptions { PartitionKey = new PartitionKey((object)person.Category ?? Undefined.Value) });
                Console.WriteLine($"Deleted in {stopwatch.ElapsedMilliseconds} ms.");
            }
        }
    }

    public class Person {
        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string Id { get; set; } // nikoliv integer!
        public string FirstName { get; set; }
        public string Surname { get; set; }

        [JsonProperty(PropertyName = "category")] // case sensitive!
        public int? Category { get; set; }

        [JsonProperty(PropertyName = "_self", NullValueHandling = NullValueHandling.Ignore)]
        public string Self { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }

    }
}
