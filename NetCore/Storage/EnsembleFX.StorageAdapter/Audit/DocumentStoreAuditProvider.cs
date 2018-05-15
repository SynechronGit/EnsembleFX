using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Audit
{
    /// <summary>
    /// Provides Audit storage on MongoDB. 
    /// </summary>
    /// <typeparam name="T">Entity to audit</typeparam>
    public class DocumentStoreAuditProvider<T> : IAuditProvider<T> where T : class
    {
        IDocumentStorageAdapter<AuditTable> dsStorageAdapter = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsStorageAdapter"></param>
        public DocumentStoreAuditProvider(IDocumentStorageAdapter<AuditTable> dsStorageAdapter) {
            this.dsStorageAdapter = dsStorageAdapter;
        }

        /// <summary>
        /// Add audit logs with delta changes
        /// </summary>
        /// <param name="action">Audit action <see cref="AuditAction"/>.</param>
        /// <param name="id">Key Id for entity</param>
        /// <param name="before">Entity before changes occured. You must pass empty instance when action is Add.</param>
        /// <param name="after">Entity after changes occured. You must pass empty instance when action is Delete.</param>
        public async Task AddAuditLogAsync(AuditAction action, string id, T before, T after)
        {
            Validate(action, id, before, after);

            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = 100;
            compareLogic.Config.CaseSensitive = false;
            compareLogic.Config.TreatStringEmptyAndNullTheSame = true;

            ComparisonResult comapreResults = compareLogic.Compare(before, after);

            List<AuditDelta> deltaList = new List<AuditDelta>();
            foreach (var change in comapreResults.Differences)
            {
                AuditDelta delta = new AuditDelta
                {
                    FieldName = change.PropertyName.Replace(change.ParentPropertyName + ".", string.Empty),
                    Value = change.Object2Value
                };
                deltaList.Add(delta);
            }

            AuditTable auditTable = new AuditTable()
            {
                _id = Guid.NewGuid(),
                EntityId = id,
                EntityName = typeof(T).Name.ToLower(),
                AuditActionType = (int)action,
                DateTimeStamp = DateTime.UtcNow,
                Changes = JsonConvert.SerializeObject(deltaList)
            };

            await dsStorageAdapter.AddAsync(auditTable);
        }

        private static void Validate(AuditAction action, string id, T before, T after)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "Cannot be null");
            }
            if (after == default(T))
            {
                throw new ArgumentNullException(nameof(after), $"Cannot be null when Audtion action is {action.ToString()}. Pass empty instance in case {AuditAction.Add.ToString()} is used.");
            }
            if (before == default(T))
            {
                throw new ArgumentNullException(nameof(before), $"Cannot be null when Audtion action is {action.ToString()}.  Pass empty instance in case {AuditAction.Delete.ToString()} is used.");
            }
        }
    }
}
