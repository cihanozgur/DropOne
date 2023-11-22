using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;

public static class CihanUtility
{
    public static GameObject PickObject2D(Vector2 screenPos, LayerMask layerMask)
    {
        Ray ray2D = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray2D, 1000, layerMask);
        if (hit2D)
        {
            return hit2D.collider.gameObject;
        }
        else
        {
            return null;
        }
    }


    public static Vector3 GetWorldPosDirectionZ(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // we solve for intersection with y = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint(t);
    }

    public static Vector3 GetWorldPosDirectionY(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // we solve for intersection with y = 0 plane
        float t = -ray.origin.y / ray.direction.y;

        return ray.GetPoint(t);
    }

    // Return the GameObject at the given screen position, or null if no valid object was found
    public static GameObject PickObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public static Vector3 PickObjectHitPoint(Vector2 screenPos, LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public static GameObject PickObject(Vector2 screenPos, out Vector3 hitPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hitPoint = hit.point;
            return hit.collider.gameObject;
        }

        hitPoint = Vector3.zero;
        return null;
    }

    public static GameObject PickObject(Vector2 screenPos, out RaycastHit _raycastHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _raycastHit = hit;
            return hit.collider.gameObject;
        }

        _raycastHit = new RaycastHit();
        return null;
    }

    public static GameObject PickObject(Vector2 screenPos, LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public static GameObject PickObject(Vector3 worldPos, Vector3 direction, LayerMask layerMask)
    {
        RaycastHit hit;

        if (Physics.Raycast(worldPos, direction, out hit, 1000, layerMask))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    public static Transform FindInAllChild(this Transform transform, string _name)
    {
        Transform[] _transforms = transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < _transforms.Length; i++)
        {
            if (_transforms[i].name == _name)
            {
                return _transforms[i];
            }
        }

        return null;
    }

    public static Texture2D ConvertToGrayscale(Texture2D graph, float value = 1)
    {
        Texture2D grayImg = new Texture2D(graph.width, graph.height);
        grayImg.SetPixels(graph.GetPixels());
        grayImg.Apply();

        Color32[] pixels = grayImg.GetPixels32();
        for (int x = 0; x < grayImg.width; x++)
        {
            for (int y = 0; y < grayImg.height; y++)
            {
                Color32 pixel = pixels[x + y * grayImg.width];
                int p = ((256 * 256 + pixel.r) * 256 + pixel.b) * 256 + pixel.g;
                int b = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int g = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int r = p % 256;
                float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
                l = l * value;
                Color c = new Color(l, l, l, 1);
                grayImg.SetPixel(x, y, c);
            }
        }

        grayImg.Apply();
        return grayImg;
    }


    public static void EditorPause(bool _b = true)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = _b;
#endif
    }

    public static LaunchData CalculateLaunchData(Vector3 _targetPos, Vector3 _currentPos, float _h)
    {
        float _displacementY = _targetPos.y - _currentPos.y;
        Vector3 displacementXZ = new Vector3(_targetPos.x - _currentPos.x, 0, _targetPos.z - _currentPos.z);
        float time = Mathf.Sqrt(-2 * _h / Physics.gravity.y) +
                     Mathf.Sqrt(2 * (_displacementY - _h) / Physics.gravity.y);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * _h);
        Vector3 velocityXZ = displacementXZ / time;
        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(Physics.gravity.y), time);
    }


    public struct LaunchData
    {
        public Vector3 initialVelocity;
        public float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    public static Quaternion AngVelToDeriv(Quaternion Current, Vector3 AngVel)
    {
        var Spin = new Quaternion(AngVel.x, AngVel.y, AngVel.z, 0f);
        var Result = Spin * Current;
        return new Quaternion(0.5f * Result.x, 0.5f * Result.y, 0.5f * Result.z, 0.5f * Result.w);
    }

    public static Vector3 DerivToAngVel(Quaternion Current, Quaternion Deriv)
    {
        var Result = Deriv * Quaternion.Inverse(Current);
        return new Vector3(2f * Result.x, 2f * Result.y, 2f * Result.z);
    }

    public static Quaternion IntegrateRotation(Quaternion Rotation, Vector3 AngularVelocity, float DeltaTime)
    {
        if (DeltaTime < Mathf.Epsilon) return Rotation;
        var Deriv = AngVelToDeriv(Rotation, AngularVelocity);
        var Pred = new Vector4(
            Rotation.x + Deriv.x * DeltaTime,
            Rotation.y + Deriv.y * DeltaTime,
            Rotation.z + Deriv.z * DeltaTime,
            Rotation.w + Deriv.w * DeltaTime
        ).normalized;
        return new Quaternion(Pred.x, Pred.y, Pred.z, Pred.w);
    }

    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time)
    {
        if (Time.deltaTime < Mathf.Epsilon) return rot;
        // account for double-cover
        var Dot = Quaternion.Dot(rot, target);
        var Multi = Dot > 0f ? 1f : -1f;
        target.x *= Multi;
        target.y *= Multi;
        target.z *= Multi;
        target.w *= Multi;
        // smooth damp (nlerp approx)
        var Result = new Vector4(
            Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
            Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
            Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
            Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
        ).normalized;

        // ensure deriv is tangent
        var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
        deriv.x -= derivError.x;
        deriv.y -= derivError.y;
        deriv.z -= derivError.z;
        deriv.w -= derivError.w;

        return new Quaternion(Result.x, Result.y, Result.z, Result.w);
    }

    public static Sequence DOJumpZ(
        this Transform target,
        Vector3 endValue,
        float jumpPower,
        int numJumps,
        float duration,
        bool snapping = false)
    {
        if (numJumps < 1)
            numJumps = 1;
        float startPosZ = target.position.z;
        float offsetZ = -1f;
        bool offsetZSet = false;
        Sequence s = DOTween.Sequence();
        Tween yTween = (Tween)DOTween
            .To((DOGetter<Vector3>)(() => target.position), (DOSetter<Vector3>)(x => target.position = x),
                new Vector3(0.0f, 0.0f, jumpPower), duration / (float)(numJumps * 2))
            .SetOptions(AxisConstraint.Z, snapping).SetEase<Tweener>(Ease.OutQuad).SetRelative<Tweener>()
            .SetLoops<Tweener>(numJumps * 2, LoopType.Yoyo)
            .OnStart<Tweener>((TweenCallback)(() => startPosZ = target.position.z));
        s.Append((Tween)DOTween
                .To((DOGetter<Vector3>)(() => target.position), (DOSetter<Vector3>)(x => target.position = x),
                    new Vector3(endValue.x, 0.0f, 0.0f), duration).SetOptions(AxisConstraint.X, snapping)
                .SetEase<Tweener>(Ease.Linear))
            .Join((Tween)DOTween
                .To((DOGetter<Vector3>)(() => target.position), (DOSetter<Vector3>)(x => target.position = x),
                    new Vector3(0.0f, endValue.y, 0.0f), duration).SetOptions(AxisConstraint.Y, snapping)
                .SetEase<Tweener>(Ease.Linear)).Join(yTween).SetTarget<Sequence>((object)target)
            .SetEase<Sequence>(DOTween.defaultEaseType);
        yTween.OnUpdate<Tween>((TweenCallback)(() =>
        {
            if (!offsetZSet)
            {
                offsetZSet = true;
                offsetZ = s.isRelative ? endValue.y : endValue.z - startPosZ;
            }

            Vector3 position = target.position;
            position.z += DOVirtual.EasedValue(0.0f, offsetZ, yTween.ElapsedPercentage(), Ease.OutQuad);
            target.position = position;
        }));
        return s;
    }

    public static Sequence DOLocalJumpZ(
        this Transform target,
        Vector3 endValue,
        float jumpPower,
        int numJumps,
        float duration,
        bool snapping = false)
    {
        if (numJumps < 1)
            numJumps = 1;
        float startPosZ = target.localPosition.z;
        float offsetZ = -1f;
        bool offsetZSet = false;
        Sequence s = DOTween.Sequence();
        Tween yTween = (Tween)DOTween
            .To((DOGetter<Vector3>)(() => target.localPosition), (DOSetter<Vector3>)(x => target.localPosition = x),
                new Vector3(0.0f, 0.0f, jumpPower), duration / (float)(numJumps * 2))
            .SetOptions(AxisConstraint.Z, snapping).SetEase<Tweener>(Ease.OutQuad).SetRelative<Tweener>()
            .SetLoops<Tweener>(numJumps * 2, LoopType.Yoyo)
            .OnStart<Tweener>((TweenCallback)(() => startPosZ = target.localPosition.z));
        s.Append((Tween)DOTween
                .To((DOGetter<Vector3>)(() => target.localPosition), (DOSetter<Vector3>)(x => target.localPosition = x),
                    new Vector3(endValue.x, 0.0f, 0.0f), duration).SetOptions(AxisConstraint.X, snapping)
                .SetEase<Tweener>(Ease.Linear))
            .Join((Tween)DOTween
                .To((DOGetter<Vector3>)(() => target.localPosition), (DOSetter<Vector3>)(x => target.localPosition = x),
                    new Vector3(0.0f, endValue.y, 0.0f), duration).SetOptions(AxisConstraint.Y, snapping)
                .SetEase<Tweener>(Ease.Linear)).Join(yTween).SetTarget<Sequence>((object)target)
            .SetEase<Sequence>(DOTween.defaultEaseType);
        yTween.OnUpdate<Tween>((TweenCallback)(() =>
        {
            if (!offsetZSet)
            {
                offsetZSet = true;
                offsetZ = s.isRelative ? endValue.y : endValue.z - startPosZ;
            }

            Vector3 position = target.position;
            position.z += DOVirtual.EasedValue(0.0f, offsetZ, yTween.ElapsedPercentage(), Ease.OutQuad);
            target.position = position;
        }));
        return s;
    }

    public static Sequence DOJumpScale(
        this Transform target,
        Vector3 endValue,
        Vector3 jumpValue,
        int numJumps,
        float duration,
        bool snapping = false)
    {
        Sequence sequence = DOTween.Sequence();

        Vector3 originalScale = target.localScale;

        for (int i = 0; i < numJumps; i++)
        {
            // Calculate the jump scale based on the current jump index
            Vector3 jumpScale = originalScale + jumpValue * (i + 1);

            // Add a scale tween for the current jump
            Tweener jumpTween = target.DOScale(jumpScale, duration / (numJumps * 2))
                .SetEase(Ease.OutQuad);

            sequence.Append(jumpTween);

            // Add a scale tween for returning to the original scale
            Tweener returnTween = target.DOScale(originalScale, duration / (numJumps * 2))
                .SetEase(Ease.InQuad);

            sequence.Append(returnTween);
        }

        // Add a final scale tween to the end value
        Tweener endTween = target.DOScale(endValue, duration / (numJumps * 2))
            .SetEase(Ease.OutQuad);

        sequence.Append(endTween);

        if (snapping)
        {
            sequence.SetUpdate(UpdateType.Normal, true);
        }

        return sequence;
    }

    public static void Log<T>(this List<T> _list)
    {
        string result = "List:{ ";
        foreach (T item in _list)
        {
            result += item.ToString() + ", ";
        }

        Debug.Log(result + "}, {Count:" + _list.Count + "}");
    }


    public static Vector2 CihanCalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint)
    {
        Vector2 contentSize = scrollView.content.rect.size;
        Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
        Vector2 contentScale = scrollView.content.localScale;

        contentSize.Scale(contentScale);
        focusPoint.Scale(contentScale);

        Vector2 scrollPosition = scrollView.normalizedPosition;
        if (scrollView.horizontal && contentSize.x > viewportSize.x)
            scrollPosition.x = Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
        if (scrollView.vertical && contentSize.y > viewportSize.y)
            scrollPosition.y = Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));
        return scrollPosition;
    }

    public static Vector2 CihanCalculateFocusedScrollPosition(this ScrollRect scrollView, RectTransform item)
    {
        Vector2 itemCenterPoint =
            scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));

        Vector2 contentSizeOffset = scrollView.content.rect.size;
        contentSizeOffset.Scale(scrollView.content.pivot);

        return scrollView.CihanCalculateFocusedScrollPosition(itemCenterPoint + contentSizeOffset);
    }

    public static void CihanFocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint)
    {
        scrollView.normalizedPosition = scrollView.CihanCalculateFocusedScrollPosition(focusPoint);
    }

    public static void CihanFocusOnItem(this ScrollRect scrollView, RectTransform item)
    {
        scrollView.normalizedPosition = scrollView.CihanCalculateFocusedScrollPosition(item);
    }

    private static IEnumerator CihanLerpToScrollPositionCoroutine(this ScrollRect scrollView,
        Vector2 targetNormalizedPos, float speed)
    {
        Vector2 initialNormalizedPos = scrollView.normalizedPosition;

        float t = 0f;
        while (t < 1f)
        {
            scrollView.normalizedPosition =
                Vector2.LerpUnclamped(initialNormalizedPos, targetNormalizedPos, 1f - (1f - t) * (1f - t));

            yield return null;
            t += speed * Time.unscaledDeltaTime;
        }

        scrollView.normalizedPosition = targetNormalizedPos;
    }

    public static IEnumerator CihanFocusAtPointCoroutine(this ScrollRect scrollView, Vector2 focusPoint, float speed)
    {
        yield return scrollView.CihanLerpToScrollPositionCoroutine(
            scrollView.CihanCalculateFocusedScrollPosition(focusPoint), speed);
    }

    public static IEnumerator CihanFocusOnItemCoroutine(this ScrollRect scrollView, RectTransform item, float speed)
    {
        yield return scrollView.CihanLerpToScrollPositionCoroutine(scrollView.CihanCalculateFocusedScrollPosition(item),
            speed);
    }


    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetHeight(this RectTransform rt, float height)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }


    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void SetWidth(this RectTransform rt, float width)
    {
        rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
    }


    public static Vector2 WorldToCanvasPosition(RectTransform canvasRectTransform, Vector3 worldPosition)
    {
        // Get the screen position of the world position
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Convert screen position to canvas position
        Vector2 canvasPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosition, null,
            out canvasPosition);

        return canvasPosition;
    }
}