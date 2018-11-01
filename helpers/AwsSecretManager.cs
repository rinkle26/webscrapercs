using System;
using System.IO;
using System.Text;
using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace WebScraperModularized.helpers
{
    public static class AwsSecretManager
    {   
        public static string Get(string secretName)
        {
            var config = new AmazonSecretsManagerConfig {RegionEndpoint = RegionEndpoint.USEast1};
            var credentials = new StoredProfileAWSCredentials("getSecretForRdsUser");
            var client = new AmazonSecretsManagerClient(credentials,config);

            var request = new GetSecretValueRequest
            {
                SecretId = secretName
            };

            GetSecretValueResponse response = null;
            try
            {
                response = client.GetSecretValueAsync(request).Result;
            }
            catch (ResourceNotFoundException)
            {
                Console.WriteLine("The requested secret " + secretName + " was not found");
            }
            catch (InvalidRequestException e)
            {
                Console.WriteLine("The request was invalid due to: " + e.Message);
            }
            catch (InvalidParameterException e)
            {
                Console.WriteLine("The request had invalid params: " + e.Message);
            }

            return response?.SecretString;
        }
    }
}