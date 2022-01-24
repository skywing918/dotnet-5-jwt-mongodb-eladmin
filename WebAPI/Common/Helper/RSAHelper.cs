namespace WebAPI.Common.Helper
{
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Security;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    public class RSAHelper
    {
        public static string decryptByPrivateKey(string privateKey, string cipherText)
        {
            var newKey = RsaPrivateKeyJava2DotNet(privateKey);
            var rsa = RSA.Create();
            FromXmlStringExtensions(rsa, newKey);
            return Encoding.UTF8.GetString(rsa.Decrypt(Convert.FromBase64String(cipherText), RSAEncryptionPadding.Pkcs1));
        }
        
        /// <summary>
        /// private key ，java->.net
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static string RsaPrivateKeyJava2DotNet(string privateKey)
        {
            if (string.IsNullOrEmpty(privateKey))
            {
                return string.Empty;
            }
            var privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));
            return
                $"<RSAKeyValue><Modulus>{Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned())}</Modulus><Exponent>{Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned())}</Exponent><P>{Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned())}</P><Q>{Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned())}</Q><DP>{Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned())}</DP><DQ>{Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned())}</DQ><InverseQ>{Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned())}</InverseQ><D>{Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned())}</D></RSAKeyValue>";
        }

        /// <summary>
        /// 扩展FromXmlString
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="xmlString"></param>
        private static void FromXmlStringExtensions(RSA rsa, string xmlString)
        {
            var parameters = new RSAParameters();

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus":
                            parameters.Modulus = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "Exponent":
                            parameters.Exponent = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "P":
                            parameters.P = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "Q":
                            parameters.Q = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "DP":
                            parameters.DP = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "DQ":
                            parameters.DQ = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "InverseQ":
                            parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                        case "D":
                            parameters.D = (string.IsNullOrEmpty(node.InnerText)
                                ? null
                                : Convert.FromBase64String(node.InnerText));
                            break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }        
    }   
}