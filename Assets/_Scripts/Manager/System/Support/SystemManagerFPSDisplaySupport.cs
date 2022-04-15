using UnityEngine;
using UnityEngine.UI;

namespace KitAR.Manager.System.Support
{
    public class SystemManagerFPSDisplaySupport : MonoBehaviour
    {
		private Canvas canvas;
		private CanvasScaler canvasScaler;
		private RectTransform contentHolder;

		private Text text;
		private float deltaTime = 0.0f;

        private void Awake()
        {
			canvas = gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 500;

			canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920, 1080);
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

			GameObject go = new GameObject("ContentHolder");
			go.transform.SetParent(transform);
			
			contentHolder = go.AddComponent<RectTransform>();
			contentHolder.anchorMin = new Vector2(0, 1);
			contentHolder.anchorMax = new Vector2(0, 1);
			contentHolder.pivot = new Vector2(0, 1);
			contentHolder.anchoredPosition3D = new Vector3(200, -200, 0);
			contentHolder.sizeDelta = new Vector2(300, 100);

			Image image = contentHolder.gameObject.AddComponent<Image>();
			image.color = Color.black;

			text = new GameObject("Text").AddComponent<Text>();
			text.font = Font.CreateDynamicFontFromOSFont("Arial", 30);
			text.fontSize = 30;
			text.alignment = TextAnchor.MiddleCenter;
			text.rectTransform.SetParent(image.transform);
			text.rectTransform.anchoredPosition3D = Vector3.zero;
			text.rectTransform.anchorMin = Vector2.zero;
			text.rectTransform.anchorMax = Vector2.one;
			text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
		}

        private void Update()
		{
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

			float msec = deltaTime * 1000.0f;
			float fps = 1.0f / deltaTime;

			string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

			this.text.text = text;
		}
	}
}
