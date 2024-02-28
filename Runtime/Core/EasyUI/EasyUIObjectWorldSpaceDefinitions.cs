using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mona.SDK.Core.Body;


namespace Mona.SDK.Core.EasyUI
{
    [RequireComponent(typeof(Canvas))]
    public class EasyUIObjectWorldSpaceDefinitions : MonoBehaviour
    {
        [SerializeField] private Transform _rootPanelTransform;
        [SerializeField] private float _rootScaleModifier = 0.2f;
        [SerializeField] private float _uiScaleModifier = 1f;
        [SerializeField] private float _displayOffset = 0.5f;
        [SerializeField] private bool _maintainUniformScale;
        [SerializeField] private EasyUIObjectSpaceDisplayArea[] _displayAreas;

        private float _oldUIScaleModifier;
        private Vector3 _oldObjectSize;
        private Vector3 _initialRootScale;
        private Renderer _objectRenderer;
        private Camera _mainCamera;
        private Canvas _canvas;

        public bool UIScaleChanged => _oldUIScaleModifier != _uiScaleModifier;
        public float RootScaleModifier { get => _rootScaleModifier; set => _rootScaleModifier = value; }
        public float UIScaleModifier { get => _uiScaleModifier; set => _uiScaleModifier = value; }

        private bool ObjectSizeChanged => _objectRenderer != null && _oldObjectSize != ObjectSize;
        private Vector3 ObjectSize => _objectRenderer ? _objectRenderer.bounds.size : Vector3.one;
        private float Height => ObjectSize.y;
        private float DisplayOffset => Height + _displayOffset;
        
        public bool MaintainUniformScale
        {
            get => _maintainUniformScale;
            set
            {
                if (!value && _rootPanelTransform)
                    _rootPanelTransform.localScale = Vector3.one * _rootScaleModifier;

                _maintainUniformScale = value;
            }
        }

        void Start()
        {
            _canvas = GetComponent<Canvas>();

            SetObjectRenderer();

            if (_rootPanelTransform)
                _initialRootScale = _rootPanelTransform.localScale;

            if (_objectRenderer != null)
                SetDisplayOffsets();
        }

        void Update()
        {
            if (!_mainCamera)
                FindSceneCamera();

            if (ObjectSizeChanged)
                SetDisplayOffsets();

            if (UIScaleChanged)
                SetUIScale();

            LookAtCamera();
            TryMaintainApparentSize();
        }

        private void LookAtCamera()
        {
            _rootPanelTransform.LookAt(_rootPanelTransform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
        }

        private void TryMaintainApparentSize()
        {
            if (!_maintainUniformScale)
                return;

            float distance = Vector3.Distance(transform.position, _mainCamera.transform.position);
            _rootPanelTransform.localScale = _initialRootScale * distance * _rootScaleModifier;
        }

        private void SetObjectRenderer()
        {
            IMonaBody monaBody = GetComponentInParent<IMonaBody>();

            if (monaBody == null)
                return;

            Transform bodyTF = monaBody.Transform;

            if (bodyTF == null)
                return;

            _objectRenderer = bodyTF.GetComponent<Renderer>();

            if (_objectRenderer == null)
                _objectRenderer = bodyTF.GetComponentInChildren<Renderer>();
        }

        private void SetDisplayOffsets()
        {
            _oldObjectSize = ObjectSize;

            SetCanvasPositionToObjectCenter();

            for (int i = 0; i < _displayAreas.Length; i++)
                _displayAreas[i].SetOffset(DisplayOffset);
        }

        private void SetCanvasPositionToObjectCenter()
        {
            if (!_objectRenderer)
                return;  

            Vector3 center = _objectRenderer.bounds.center;
            Vector3 localCenter = transform.InverseTransformPoint(center);
            transform.localPosition = localCenter;
        }

        public void SetUIScale()
        {
            _oldUIScaleModifier = _uiScaleModifier;

            for (int i = 0; i < _displayAreas.Length; i++)
                _displayAreas[i].SetScale(_uiScaleModifier);
        }

        public void PlaceElementInObjectUI(IEasyUINumericalDisplay variable)
        {
            for (int i = 0; i < _displayAreas.Length; i++)
            {
                if (_displayAreas[i].ScreenZone.ObjectPlacement == variable.ObjectPosition)
                {
                    _displayAreas[i].ScreenZone.AddVariable(variable);
                }
            }
        }

        private void FindSceneCamera()
        {
            MonaBodyPart[] monaBodyParts = FindObjectsOfType<MonaBodyPart>();

            foreach (MonaBodyPart part in monaBodyParts)
            {
                if (!part.MonaTags.Contains("Camera"))
                    continue;

                Camera cameraComponent = part.GetComponent<Camera>();

                if (cameraComponent == null)
                    continue;

                _mainCamera = cameraComponent;

                if (_canvas)
                    _canvas.worldCamera = _mainCamera;

                break;
            }
        }
    }
}
