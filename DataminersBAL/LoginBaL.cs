using DataminersDAL.Repositories;
using DataminersModel;
using System.Collections;
using System.Data;
using System.DirectoryServices;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;


namespace DataminersBAL
{
    public class LoginBaL
    {
        private string UserIdq = string.Empty;
        private string Location = string.Empty;
        private string email = string.Empty;

        private readonly LoginRepository _loginRepository;

        public LoginBaL(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public bool AuthenticateUser(string path, string user, string pass)
        {
            var de = new System.DirectoryServices.DirectoryEntry(path, user, pass);
            try
            {
                // var de = new DirectoryEntry(path, user, pass);
                var ds = new DirectorySearcher(de);
                ds.FindOne();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                de.Close();
            }
        }

        public static DataTable convertStringToDataTable(string data)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EmployeeID");
            foreach (string value in data.Split('#'))
            {
                dt.Rows.Add(value);
            }
            return dt;
        }

        public DataSet ITDBLogin(string ntLoginID, string password)
        {
            return _loginRepository.ITDBLogin(ntLoginID, password);
        }

        public DataSet BFILLogin(LoginModel data)
        {
            string finalresult = string.Empty;
            DataSet ds = new DataSet();


            //LogService("Step4");

            try
            {

                //LogService("Step5");


                string StrUser = "bfil\\" + data.Username;
                string[] Result = StrUser.Split(new char[] { '\\' });
                string StrDomainName;
                string StrName;
                if (Result.Length > 1)
                {
                    StrDomainName = Result[0];
                    StrName = Result[1];
                    if (StrDomainName == "bfil")
                    {

                        string Ldapconnectionbfil = "ldaps://DCPRAD03/DC=bfil,DC=local";
                        DirectorySearcher dssearch = new DirectorySearcher(Ldapconnectionbfil)
                        {
                            Filter = "(sAMAccountName=" + StrName + ")"
                        };

                        dssearch.SearchRoot.Path = "LDAP://DC=BFIL,DC=local";
                        SearchResult sresult = dssearch.FindOne();

                        Dictionary<string, ArrayList> userDetailsDic = new Dictionary<string, ArrayList>();

                        if (sresult != null)
                        {
                            foreach (string propertyName in sresult.Properties.PropertyNames)
                            {
                                ArrayList Results = new ArrayList();

                                foreach (object myCollection in sresult.Properties[propertyName])
                                {
                                    Results.Add(myCollection);
                                }

                                userDetailsDic.Add(propertyName, Results);
                            }

                            foreach (KeyValuePair<string, ArrayList> detail in userDetailsDic)
                            {
                                foreach (object value in detail.Value)
                                {
                                    if (detail.Key == "description")
                                    {
                                        UserIdq = value.ToString();
                                        // //LogService("AuthenticateUser " + UserIdq);

                                    }
                                    if (detail.Key == "mail")
                                    {
                                        email = value.ToString();
                                        //  //LogService("AuthenticateUser " + email);
                                    }
                                    if (detail.Key == "physicaldeliveryofficename")
                                    {
                                        Location = value.ToString();

                                    }

                                }

                            }


                            //LogService("Step6");


                            System.DirectoryServices.DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                            string ldappath = dsresult.Path;


                            string LineOfText;

                            string[] ArryText;
                            string Split_LdapPath = "";
                            string word = ldappath;

                            word = word.Replace("sksindia", "BFIL");
                            LineOfText = word;
                            ArryText = LineOfText.Split(new[] { "//" }, StringSplitOptions.None);

                            Split_LdapPath = ArryText[1];

                            ///DCPRADC
                            //string FinalPath = "LDAP://DCPRAD03/" + Split_LdapPath;
                            string FinalPath = "LDAP://DCPRAD03/" + Split_LdapPath;
                            // //LogService("AuthenticateUser " + FinalPath);

                            bool aa = AuthenticateUser(FinalPath, data.Username, data.Password);


                            //LogService("Step7");

                            if (aa)
                            {
                                ////LogService("Step8");

                                string uid = UserIdq;
                                if (Location == "HEAD OFFICE" || Location == "Head Office")
                                {
                                    ds = _loginRepository.Login(UserIdq);
                                    return ds;
                                }
                                //else
                                //{
                                //    //ds = _loginRepostiory.login("Forms", UserIdq);

                                //    ////ds = loginDal.LoginBranches(UserIdq);
                                //    //return ds;
                                //}
                            }
                            else
                            {
                                finalresult = "E203#Invalid Password";
                            }
                        }
                        else
                        {
                            finalresult = "E204#Invalid User Name";
                        }
                    }
                    else
                    {
                        finalresult = "E205#Please prefix BFIL to userName";
                    }
                }
            }


            catch (System.Threading.ThreadAbortException ex)
            {
                finalresult = "E206#" + ex.Message;

            }
            catch (Exception ex)
            {
                finalresult = "E207#" + ex.Message;


            }
            DataTable dtt = convertStringToDataTable(finalresult);
            ds.Tables.Add(dtt);
            return ds;


        }
    }
}
