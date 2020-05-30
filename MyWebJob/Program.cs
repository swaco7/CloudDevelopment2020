using System;
using System.Configuration;



namespace MyWebJob {
    class Program {
		static void Main(string[] args) {
			while (true) {
				Console.WriteLine("Checking for new e-mails to be sent...");

				using (var conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["MainDatabase"].ConnectionString)) {
					conn.Open();

					var cmd = new System.Data.SqlClient.SqlCommand("SELECT * FROM EmailQueue WHERE Sent IS NULL", conn);

					using (var reader = cmd.ExecuteReader()) {
						while (reader.Read()) {
							Console.WriteLine($"Email ID:{reader["ID"]} found...");

							using (var smtpClient = new System.Net.Mail.SmtpClient()) {
								smtpClient.Host = ConfigurationManager.AppSettings["SmtpHost"];
								smtpClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUsername"], ConfigurationManager.AppSettings["SmtpPassword"]);

								smtpClient.Send(
									from: ConfigurationManager.AppSettings["SmtpFrom"],
									recipients: reader["Recipient"].ToString(),
									subject: reader["Subject"].ToString(),
									body: reader["Body"].ToString());

								// TODO: Write EmqilQueue.Sent to DB

								Console.WriteLine($"Email ID:{reader["ID"]} sent...");
							}
						}
					}
				}

				System.Threading.Thread.Sleep(30_000); // 30sec
			}
		}
	}
}
