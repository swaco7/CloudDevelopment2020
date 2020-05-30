using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KeyVault1 {
	class Program {
		// Vytvořte Key Vault.
		// Vytvořte v něm secret pojmenovaný "MySecret" s nějakou (ne zas tak tajnou :-) hodnotou).
		// Vytvořte v něm klíč pojmenovaný "MyKey".

		// Vytvořte v Azure Active Directory účet aplikace (WebAPI, nikoliv Native).
		// Vytvořte pro aplikaci klíč.

		// Zpět do Key Vault, přidělte aplikaci oprávnění práci se Secrets (Read) a Keys (Encrypt, Decrypt).
		// Doplňte údaje do aplikace.

		static async Task Main(string[] args) {
			string vaultAddress = "https://mykeyvaultrs.vault.azure.net/";
			string secretName = "MySecretRS"; // Azure Key Vault Secret Name
			string keyName = "MyKeyRS"; // Azure Key Vault Key Name

			KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetAccessToken));

			var secret = await keyVaultClient.GetSecretAsync(vaultAddress, secretName);
			Console.WriteLine(secret/*.Value*/);
			Console.WriteLine();

			var encrypted = await keyVaultClient.EncryptAsync(vaultAddress, keyName, "", JsonWebKeyEncryptionAlgorithm.RSA15, System.Text.Encoding.UTF8.GetBytes("Live demo on MFF!"));
			var decrypted = await keyVaultClient.DecryptAsync(vaultAddress, keyName, "", JsonWebKeyEncryptionAlgorithm.RSA15, encrypted.Result);

			Console.WriteLine(String.Join(" ", encrypted.Result.Select(item => item.ToString("X2"))));
			Console.WriteLine();

			Console.WriteLine(String.Join("", System.Text.Encoding.UTF8.GetString(decrypted.Result)));
			Console.WriteLine();
			Console.ReadKey();
		}

		public static async Task<string> GetAccessToken(string authority, string resource, string scope) {
			var clientId = "378de4b9-e982-429a-9e3e-5dff7d2c336f"; // Application ID
			var clientSecret = "1l92pG0zp_zttELAAvJ3~i~465.eo86y2-"; // Application Key
			ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);

			var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
			var result = await context.AcquireTokenAsync(resource, clientCredential);

			return result.AccessToken;
		}
	}
}
