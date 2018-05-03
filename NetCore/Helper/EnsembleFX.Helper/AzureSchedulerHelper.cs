//TODO - Need to find replacement solution for scheduler operations

//using Microsoft.Azure;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using Microsoft.Azure.Common.Authentication.Models;
//using Microsoft.Azure.Management.Scheduler;
//using Microsoft.Azure.Management.Scheduler.Models;
//using Microsoft.IdentityModel.Clients.ActiveDirectory;
//using Microsoft.Rest;
////using EnsembleFX.Shared;
//using Microsoft.Rest.Azure;

//namespace EnsembleFX.Helper
//{
//    public class AzureSchedulerHelper
//    {
//        private AzureEnvironment azureEnvironment;
//        private SchedulerManagementClient schedulerManagementClient;
//        string resourceGroup = null;
//        string location = null;

//        public AzureSchedulerHelper()
//        {

//            // Set Environment - Choose between Azure public cloud, china cloud and US govt. cloud
//            azureEnvironment = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];

//            // Get the credentials
//            var tokenCloudCreds = GetCredsFromServicePrincipal();
//            var tokenCreds = new TokenCredentials(tokenCloudCreds.Token);

//            // Use credentials to create Scheduler managment client.
//            schedulerManagementClient = new SchedulerManagementClient(azureEnvironment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager), tokenCreds)
//            {
//                SubscriptionId = ConfigurationManager.AppSettings["AzureSubscriptionId"]
//            };

//            resourceGroup = ConfigurationManager.AppSettings["AzureResourceGroup"];
//            location = ConfigurationManager.AppSettings["AzureLocation"];
//        }

//        private TokenCloudCredentials GetCredsFromServicePrincipal()
//        {
//            var subscriptionId = ConfigurationManager.AppSettings["AzureSubscriptionId"];
//            var tenantId = ConfigurationManager.AppSettings["AzureTenantId"];
//            var clientId = ConfigurationManager.AppSettings["AzureClientId"];
//            var clientSecret = ConfigurationManager.AppSettings["AzureClientSecret"];

//            // Quick check to make sure we're not running with the default app.config
//            if (subscriptionId[0] == '[')
//            {
//                throw new Exception("You need to enter your appSettings in app.config to run this sample");
//            }

//            var authority = String.Format("{0}{1}", azureEnvironment.Endpoints[AzureEnvironment.Endpoint.ActiveDirectory], tenantId);
//            var authContext = new AuthenticationContext(authority);
//            var credential = new ClientCredential(clientId, clientSecret);
//            var authResult = authContext.AcquireToken(azureEnvironment.Endpoints[AzureEnvironment.Endpoint.ActiveDirectoryServiceEndpointResourceId], credential);

//            return new TokenCloudCredentials(subscriptionId, authResult.AccessToken);
//        }

//        private JobCollectionDefinition BuildJobCollecionDefinition(string jobCollectionName, string location)
//        {
//            return new JobCollectionDefinition()
//            {
//                Name = jobCollectionName,
//                Location = location,
//                Properties = new JobCollectionProperties()
//                {
//                    Sku = new Sku()
//                    {
//                        Name = SkuDefinition.Standard,
//                    },
//                    State = JobCollectionState.Enabled,
//                    Quota = new JobCollectionQuota()
//                    {
//                        MaxRecurrence = new JobMaxRecurrence()
//                        {
//                            Frequency = RecurrenceFrequency.Minute,
//                            Interval = 1,
//                        },
//                        MaxJobCount = 5
//                    }
//                }
//            };
//        }

//        private void CreateJobCollectionAndJobs(SchedulerManagementClient schedulerManagementClient, SchedulerJob schedulerJob)
//        {
            
//            var jobCollectionNamePrefix = "jc_";
//            var jobCollectionName = string.Format("{0}{1}", jobCollectionNamePrefix, schedulerJob.SchedulerJobName);

//            schedulerManagementClient.JobCollections.CreateOrUpdate(
//                resourceGroupName: resourceGroup,
//                jobCollectionName: jobCollectionName,
//                jobCollection: BuildJobCollecionDefinition(jobCollectionName, location));

//            CreateOrUpdateJob(schedulerManagementClient, resourceGroup, jobCollectionName, schedulerJob);
//        }

//        private void CreateOrUpdateJob(
//            SchedulerManagementClient schedulerManagementClient,
//            string resourceGroupName,
//            string jobCollectionName,
//            SchedulerJob schedulerJob)
//        {
//            var headers = new Dictionary<string, string>();

//            var andomHour = new Random();
//            var randomMinute = new Random();
//            var randomSecond = new Random();
//            DateTime startJobDateTime = DateTime.UtcNow;
//            JobRecurrence jobRecurrence = null;
//            if (schedulerJob.RecurringJob)
//                jobRecurrence = BuildJobRecurrence(schedulerJob);
//            else
//                startJobDateTime = schedulerJob.StartJobOn.HasValue ? schedulerJob.StartJobOn.Value.ToUniversalTime() : DateTime.UtcNow;

//            schedulerManagementClient.Jobs.CreateOrUpdate(
//                resourceGroupName,
//                jobCollectionName,
//                schedulerJob.SchedulerJobName,
//                new JobDefinition()
//                {
//                    Properties = new JobProperties()
//                    {
//                        StartTime = startJobDateTime,
//                        Action = new JobAction()
//                        {
//                            Type = JobActionType.Http,
//                            Request = new HttpRequest()
//                            {
//                                Uri = string.Format("{0}{1}", ConfigurationManager.AppSettings["apiRootAddress"], schedulerJob.HttpActionUrl),
//                                Method = "GET",
//                            },
//                            RetryPolicy = new RetryPolicy()
//                            {
//                                RetryType = RetryType.None,
//                            }
//                        },
//                        Recurrence = jobRecurrence,
//                        State = JobState.Enabled,
//                    }
//                });
//        }

//        private JobRecurrence BuildJobRecurrence(SchedulerJob job)
//        {
//            JobRecurrence jobRecurrence = new JobRecurrence();
//            jobRecurrence.Frequency = (RecurrenceFrequency)job.Frequency;
//            jobRecurrence.Interval = job.Interval;
//            if (job.EndingType.Value == 0) { jobRecurrence.EndTime = job.EndDateTime.HasValue ? job.EndDateTime.Value.ToUniversalTime() : job.EndDateTime; }
//            else if (job.EndingType.Value == 1) { jobRecurrence.Count = job.EndJobAtOccurrence; }
//            return jobRecurrence;
//        }

//        public bool ScheduleJob(SchedulerJob job)
//        {
//            CreateJobCollectionAndJobs(schedulerManagementClient, job);
//            return true;
//        }

//        public bool DeleteJob(string schedulerJobName)
//        {
//            var jobCollectionNamePrefix = "jc_";
//            var jobCollectionName = string.Format("{0}{1}", jobCollectionNamePrefix, schedulerJobName);
//            try
//            {
//                schedulerManagementClient.JobCollections.Delete(resourceGroup, jobCollectionName);
//            }
//            catch (CloudException ex)
//            {
//                //Do nothing, error will be thrown if the job collection does not  exist
//            }
//            return true;
//        }

//        //public bool DeleteJob(object applet)
//        //{
//        //    try
//        //    {
//        //        var appletjosn = JsonConvert.SerializeObject(applet);

//        //        JObject appletJObject = JObject.Parse(appletjosn);

//        //        // To do :- check null
//        //        string jobname = appletJObject.Root["AppletTitle"].ToString().ToLower().Replace(" ", "-") + appletJObject.Root["_id"].ToString();

//        //        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
//        //        store.Open(OpenFlags.ReadWrite);

//        //        var certificate = store.Certificates.Find(X509FindType.FindByThumbprint, ConfigurationManager.AppSettings["Certificates"], false)[0];
//        //        store.Close();
//        //        var credentials = new CertificateCloudCredentials(ConfigurationManager.AppSettings["SubscriptionId"], certificate);
//        //        var schedulerClient = new SchedulerClient(ConfigurationManager.AppSettings["CloudServiceName"], ConfigurationManager.AppSettings["JobCollectionName"], credentials);

//        //        schedulerClient.Jobs.Delete(jobname);
//        //        return true;
//        //    }
//        //    catch (Exception ex)
//        //    {

//        //        return false;
//        //    }
//        //}

//    }

//}

