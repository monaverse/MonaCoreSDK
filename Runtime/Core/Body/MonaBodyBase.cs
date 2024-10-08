using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;
using Mona.SDK.Core.Utils;
using Mona.SDK.Core.Events;

namespace Mona.SDK.Core.Body
{
    public class MonaBodyBase : MonoBehaviour
    {
        public SerializableGuid guid => _guid;

        [SerializeField] private string _prefabId;
        public string PrefabId {
            get {
                //if (!string.IsNullOrEmpty(_prefabId)) return _prefabId;
                //return _guid;
                return _prefabId;
            }
            set
            {
                _prefabId = value;
            }
        }

        private bool _isSceneObject = true;
        public bool IsSceneObject
        {
            get => _isSceneObject;
            set => _isSceneObject = value;
        }

        private bool _locallyOwnedMonaBody = false;
        public bool LocallyOwnedMonaBody => _locallyOwnedMonaBody;

        private int _localPlayerId = 0;
        [SerializeField] private string _localId;

        [SerializeField] private SerializableGuid _guid;
        [SerializeField] private SerializableGuid _durableID;

        public string LocalId
        {
            get
            {
                CalculateLocalIdOnIndex();
                return _localId;
            }
            set => _localId = value;
        }

        public string RawLocalId { get => _localId; set => _localId = value; }

        public SerializableGuid Guid { get => _guid; set => _guid = value; }
        public SerializableGuid DurableId { get => _durableID; set => _durableID = value; }

        
        //Fall back for Empty Guids on Reactors
        private StringBuilder _pathNameBuilder = new StringBuilder();
        public string PathName
        {
            get
            {
                if (_pathNameBuilder.Length == 0)
                {
                    var parents = GetComponentsInParent<Transform>(true);
                    _pathNameBuilder.Append(name);
                    for (var i = 0; i < parents.Length; i++)
                    {
                        _pathNameBuilder.Append(parents[i].name);
                        if (i < parents.Length - 1)
                            _pathNameBuilder.Append(".");
                    }
                }
                return _pathNameBuilder.ToString();
            }
        }

        public void CalculateUniqueId(Dictionary<MonaBodyBase, int> uniquesIndex)
        {
            if (uniquesIndex.ContainsKey(this))
            {
                var count = uniquesIndex[this];
                if (count > 0)
                {
                    var suffix = ((_localPlayerId+1) * 100000 + count).ToString();
                    var tmpGuid = _guid.ToString();
                    _localId = string.Concat(tmpGuid.Substring(0, tmpGuid.Length - suffix.Length), suffix);
                }
                else
                    _localId = _guid;
            }
            else
                _localId = _guid;
        }

        private void CalculateLocalIdOnCount()
        {
            if (string.IsNullOrEmpty(_localId))
            {
                var uniques = new List<MonaBodyBase>(GameObject.FindObjectsOfType<MonaBodyBase>(true));

                var count = uniques.FindAll((x) => x.guid == _guid).Count;
                //if (count > 1)
                {
                    var suffix = ((_localPlayerId+1) * 100000 + count).ToString();
                    var tmpGuid = _guid.ToString();
                    _localId = string.Concat(tmpGuid.Substring(0, tmpGuid.Length - suffix.Length), suffix);
                }
                //else
                //    _localId = _guid;
            }
        }

        private void CalculateLocalIdOnIndex()
        {
            if (string.IsNullOrEmpty(_localId))
            {
                //Attempt to calculate a deterministic GUID for scene objects that have the same GUID.
                //Scenes with Reactors before this update will have an empty GUID.
                //monavox-editor has over 22000 reactors and they need to be durably identified otherwise networking does not sync correctly.
                //old spaces should be migrated to the new guid convention
                var uniques = new List<MonaBodyBase>(GameObject.FindObjectsOfType<MonaBodyBase>(true));
                uniques.Sort((a, b) =>
                {
                    var compare = a.PathName.CompareTo(b.PathName);
                    if (compare == 0) return a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex());
                    else return compare;
                });

                var count = uniques.FindIndex((x) => x.guid == _guid && x == this);
                if (count > -1)
                {
                    SetLocalId(_guid.ToString(), _localPlayerId, count);
                    //Debug.Log($"make unique {_localId}");
                }
                else
                    _localId = _guid;
            }
        }

        public void ManualMakeUnique(string tmpGuid, int playerId, int count, bool locallyOwnedMonaBody = true)
        {
            _isSceneObject = false;
            _locallyOwnedMonaBody = locallyOwnedMonaBody;
            _localPlayerId = playerId;
            _localId = null;
            _localPlayerId = playerId;
            SetLocalId(tmpGuid, _localPlayerId, count);
        }

        private void SetLocalId(string tmpGuid, int playerId, int count)
        {
            var suffix = ((playerId + 1) * 100000 + count).ToString();
            _localId = string.Concat(tmpGuid.Substring(0, tmpGuid.Length - suffix.Length), suffix);
            //Debug.Log($"{nameof(ManualMakeUnique)} {tmpGuid} {_localId}");
        }

        public void MakeUnique(int playerId, bool locallyOwnedMonaBody = true)
        {
            _isSceneObject = false;
            _locallyOwnedMonaBody = locallyOwnedMonaBody;
            _localPlayerId = playerId;
            _localId = null;
            CalculateLocalIdOnCount();
            //Debug.Log($"{nameof(MonaBody)}.{nameof(MakeUnique)} {_localId}");
        }

        public static IMonaBody Spawn(Guid guid, string prefabId, int playerId, MonaBody monaBody, bool spawn, Vector3 position, Quaternion rotation, bool locallyOwnedMonaBody = false)
        {
            var body = spawn ? (IMonaBody)GameObject.Instantiate(monaBody, Vector3.up * 10000f, Quaternion.identity) : monaBody;

            var bodies = body.Transform.GetComponentsInChildren<IMonaBody>();
            //Debug.Log($"GENERATE GUID {guid} {prefabId}");
            for (var j = 0; j < bodies.Length; j++)
            {
                var child = bodies[j];
                if (child == body)
                {
                    ((MonaBodyBase)child).PrefabId = prefabId;
                }
                else
                    ((MonaBodyBase)child).PrefabId = null;

                ((MonaBodyBase)child).Guid = new SerializableGuid(guid);
                ((MonaBodyBase)child).ManualMakeUnique(guid.ToString(), playerId, j, locallyOwnedMonaBody);
                MonaEventBus.Trigger<MonaBodyInstantiatedEvent>(new EventHook(MonaCoreConstants.MONA_BODY_INSTANTIATED), new MonaBodyInstantiatedEvent(child));
            }

            return body;
        }

        private void Reset()
        {
            EnsureUniqueDurableID();
        }

        public void EnsureUniqueDurableID()
        {
            if (_durableID.ToString() == System.Guid.Empty.ToString())
                GenerateDurableID();
        }

        private void GenerateDurableID()
        {
            _durableID = System.Guid.NewGuid();
        }
    }
}
