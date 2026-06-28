// One-click wiring for the Tier 1 "juice" pass.
// Menu: Tools > FakeOutBoss > Setup Tier 1 Juice
//
// Creates the UI objects (flash overlay, QTE timer ring, per-boss "!" icons) and
// wires every serialized reference programmatically, so no manual drag-drop is needed.
// Idempotent: re-running reuses objects it already created (matched by name).
//
// Audio note: heartbeat / alert-sting AudioSources are created empty. Drop your own
// clips onto them in the inspector (or they simply stay silent — code is null-safe).
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class JuiceSetup
{
    [MenuItem("Tools/FakeOutBoss/Setup Tier 1 Juice")]
    public static void Setup()
    {
        int wired = 0;

        // --- ScreenJuice + full-screen flash ---------------------------------
        var screenJuice = Object.FindFirstObjectByType<ScreenJuice>();
        if (screenJuice == null)
        {
            var go = new GameObject("ScreenJuice");
            Undo.RegisterCreatedObjectUndo(go, "Create ScreenJuice");
            screenJuice = go.AddComponent<ScreenJuice>();
        }

        Canvas overlay = FindOverlayCanvas();
        Image flash = null;
        if (overlay != null)
        {
            flash = FindOrCreateChildImage(overlay.transform, "DamageFlash");
            var rt = flash.rectTransform;
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            flash.color = new Color(1f, 0f, 0f, 0f);
            flash.raycastTarget = false;
            flash.transform.SetAsLastSibling();
            flash.enabled = false;
            flash.gameObject.SetActive(true);
        }

        var camT = FindGameCamera();
        var sjSO = new SerializedObject(screenJuice);
        if (flash != null) { sjSO.FindProperty("flashImage").objectReferenceValue = flash; wired++; }
        if (camT != null) { sjSO.FindProperty("shakeTarget").objectReferenceValue = camT; wired++; }
        sjSO.ApplyModifiedPropertiesWithoutUndo();

        // --- KeyChallengeManager: radial timer ring --------------------------
        var kcm = Object.FindFirstObjectByType<KeyChallengeManager>(FindObjectsInactive.Include);
        if (kcm != null)
        {
            var kcmSO = new SerializedObject(kcm);
            var keyCanvasProp = kcmSO.FindProperty("keyCanvas");
            var keyCanvas = keyCanvasProp != null ? keyCanvasProp.objectReferenceValue as RectTransform : null;
            if (keyCanvas != null)
            {
                var ring = FindOrCreateChildImage(keyCanvas, "ChallengeTimerRing");
                ring.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
                ring.type = Image.Type.Filled;
                ring.fillMethod = Image.FillMethod.Radial360;
                ring.fillOrigin = (int)Image.Origin360.Top;
                ring.fillClockwise = false;
                ring.fillAmount = 1f;
                ring.raycastTarget = false;
                var rrt = ring.rectTransform;
                rrt.anchoredPosition = Vector2.zero;
                rrt.sizeDelta = new Vector2(260, 260);
                ring.transform.SetAsFirstSibling();   // keys draw on top
                ring.gameObject.SetActive(false);

                kcmSO.FindProperty("challengeTimerRing").objectReferenceValue = ring;
                kcmSO.ApplyModifiedPropertiesWithoutUndo();
                wired++;
            }
            else Debug.LogWarning("[JuiceSetup] KeyChallengeManager.keyCanvas not assigned — skipped ring.");
        }

        // --- BarUiManager: heartbeat AudioSource -----------------------------
        var bars = Object.FindFirstObjectByType<BarUiManager>(FindObjectsInactive.Include);
        if (bars != null)
        {
            var hb = bars.GetComponent<AudioSource>();
            if (hb == null) hb = Undo.AddComponent<AudioSource>(bars.gameObject);
            hb.playOnAwake = false;
            hb.loop = true;
            hb.spatialBlend = 0f;
            var barsSO = new SerializedObject(bars);
            barsSO.FindProperty("heartbeat").objectReferenceValue = hb;
            barsSO.ApplyModifiedPropertiesWithoutUndo();
            wired++;
        }

        // --- Per-boss "!" alert icon + sting AudioSource ---------------------
        var brains = Object.FindObjectsByType<AiBrain>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var brain in brains)
        {
            var icon = FindOrCreateAlertIcon(brain.transform);
            var sting = brain.GetComponent<AudioSource>();
            if (sting == null) sting = Undo.AddComponent<AudioSource>(brain.gameObject);
            sting.playOnAwake = false;
            sting.loop = false;
            sting.spatialBlend = 0f;

            var bSO = new SerializedObject(brain);
            bSO.FindProperty("alertIcon").objectReferenceValue = icon;
            bSO.FindProperty("alertSound").objectReferenceValue = sting;
            bSO.ApplyModifiedPropertiesWithoutUndo();
            wired++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveOpenScenes();
        Debug.Log($"[JuiceSetup] Done. Wired {wired} reference groups across {brains.Length} boss(es). " +
                  "Drop heartbeat / alert-sting clips onto the new AudioSources if you want sound.");
    }

    // ---- helpers -----------------------------------------------------------

    static Canvas FindOverlayCanvas()
    {
        var canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var c in canvases)
            if (c.isRootCanvas && c.renderMode == RenderMode.ScreenSpaceOverlay) return c;
        foreach (var c in canvases) if (c.isRootCanvas) return c;
        return canvases.Length > 0 ? canvases[0] : null;
    }

    static Camera FindGameCameraComp()
    {
        var cams = Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var c in cams) if (c.CompareTag("MainCamera")) return c;
        return cams.Length > 0 ? cams[0] : null;
    }

    static Transform FindGameCamera()
    {
        var c = FindGameCameraComp();
        return c != null ? c.transform : null;
    }

    static Image FindOrCreateChildImage(Transform parent, string name)
    {
        var existing = parent.Find(name);
        if (existing != null)
        {
            var img = existing.GetComponent<Image>();
            return img != null ? img : existing.gameObject.AddComponent<Image>();
        }
        var go = new GameObject(name, typeof(RectTransform), typeof(Image));
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        go.transform.SetParent(parent, false);
        return go.GetComponent<Image>();
    }

    static GameObject FindOrCreateAlertIcon(Transform boss)
    {
        var existing = boss.Find("AlertIcon");
        if (existing != null) return existing.gameObject;

        var go = new GameObject("AlertIcon");
        Undo.RegisterCreatedObjectUndo(go, "Create AlertIcon");
        go.transform.SetParent(boss, false);
        go.transform.localPosition = new Vector3(0f, 2.6f, 0f);
        go.transform.localScale = Vector3.one * 0.5f;

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = "!";
        tmp.fontSize = 18;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(1f, 0.15f, 0.15f);
        tmp.fontStyle = FontStyles.Bold;

        // Billboard the icon toward the camera at runtime.
        go.AddComponent<FaceCamera>();

        go.SetActive(false);
        return go;
    }
}
