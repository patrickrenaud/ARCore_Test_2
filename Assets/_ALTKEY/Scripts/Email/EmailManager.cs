using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

namespace ca.altkey
{
    public class EmailManager : MonoBehaviour
    {
        public static bool _pictureEnabled = false;

        public AudioClip _cameraPhotoClip;
        public AudioClip _sendConfirmClip;
        public Camera _camera;
        public Image _flashImg;
        public RawImage _previewImg;
        public CanvasGroup _popupGroup;
        public GameObject _popup;
        public GameObject _popupConfirm;
        public GameObject _pictureContainer;
        public TMP_InputField _inputField;
        public Texture2D _screenshot;
        public MarkerFusionTrackerSample _markerTracker;

        public GameObject _blocker;
        public GameObject _loader;

        public float _flashDuration = 0.2f;

        private string _serverURL = "http://martellotour.com/dev/TourMartelloServer/";
        private string _serverMailpath = "send_mail.php";

        private AudioSource _audioSource;

        public static void Reset()
        {
            _pictureEnabled = false;
        }

        public void Start()
        {
            Init();
        }

        public bool PictureEnabled
        {
            get
            {
                return _pictureEnabled;
            }
            set
            {
                _pictureEnabled = value;
            }
        }

        public void Init()
        {
            _popup.SetActive(false);

            _audioSource = GetComponent<AudioSource>();

            _pictureContainer.SetActive(_pictureEnabled);

            _pictureEnabled = false;
        }

        public void TakePicture()
        {
            _audioSource.clip = _cameraPhotoClip;
            _audioSource.Play();

            RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
            _camera.targetTexture = rt;
            _screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            _camera.Render();
            RenderTexture.active = rt;
            _screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            _screenshot.Apply();
            _previewImg.texture = _screenshot;
            _camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            _popup.SetActive(true);
            _blocker.SetActive(true);
            _loader.SetActive(false);

            StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            _flashImg.color = Color.white;
            Color c;
            for (float f = _flashDuration; f >= 0; f -= Time.deltaTime)
            {
                c = _flashImg.color;
                c.a = f / _flashDuration;
                _popupGroup.alpha = 1 - (f / _flashDuration);
                _flashImg.color = c;
                yield return null;
            }
            _popupGroup.alpha = 1;
            _markerTracker.DisableTracking();
            c = _flashImg.color;
            c.a = 0;
            _flashImg.color = c;
        }

        public void SendEmail()
        {
            StartCoroutine(SendRoutine());
        }

        private IEnumerator SendRoutine()
        {
            _blocker.SetActive(false);
            _loader.SetActive(true);

            _audioSource.clip = _sendConfirmClip;
            _audioSource.Play();

            byte[] bytes = _screenshot.EncodeToPNG();
            WWWForm form = new WWWForm();
            form.AddField("local", LocalizationManager._lang == LocalizationManager.LangType.EN ?"EN":"FR");
            form.AddField("to", _inputField.text);
            form.AddBinaryData("image", bytes, "screenshot.png", "image/png");
    
            using(UnityWebRequest unityWebRequest = UnityWebRequest.Post(_serverURL+_serverMailpath, form))
            {
                yield return unityWebRequest.SendWebRequest();
                
                if (unityWebRequest.responseCode != 200) 
                {
                    print($"Failed to send email request: {unityWebRequest.responseCode} - {unityWebRequest.error}");
                }
                else 
                {
                    print($"Finished Uploading email request");
                }
            }

            _popup.SetActive(false);
            _loader.SetActive(false);
            _popupConfirm.SetActive(true);
        }
    }
}
