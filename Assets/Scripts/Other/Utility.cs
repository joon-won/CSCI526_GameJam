using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using CSCI526GameJam;

public enum AssetType {
    All,
    Prefab,
    ScriptableObject,
}

public static class Utility {

    /// <summary>
    /// Casts the given <paramref name="obj"/> to the specified type <typeparamref name="T"/>.
    /// Logs a warning if the casting is unsuccessful.
    /// </summary>
    public static T TryCast<T>(this object obj) where T : class {
        if (obj == null) {
            Debug.LogWarning("obj is null. ");
            return null;
        }

        if (obj is T castedObj) {
            return castedObj;
        }
        else {
            Debug.LogWarning($"{obj.GetType()} is not {typeof(T)}. ");
            return null;
        }
    }

    public static List<T> FindAllInActiveScene<T>() {
        var results = new List<T>();

        var objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in objs) {
            results.AddRange(obj.GetComponentsInChildren<T>());
        }

        Debug.Log($"{results.Count} objects of type {typeof(T)} are found. ");
        return results;
    }

    public static List<string> GetLayerNames(this LayerMask layerMask) {
        var layerNames = new List<string>();

        var layer = layerMask.value;
        for (int i = 0; i < 32; i++) // Unity has 32 layers
        {
            if ((layer & (1 << i)) != 0) {
                var layerName = LayerMask.LayerToName(i);
                if (!string.IsNullOrEmpty(layerName)) {
                    layerNames.Add(layerName);
                }
            }
        }
        return layerNames;
    }

    /// <summary>
    /// Return iteratable enum values. 
    /// </summary>
    public static IEnumerable<T> GetEnumValues<T>() {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static T Parse<T>(this string str) where T : Enum {
        return (T)Enum.Parse(typeof(T), str);
    }

    public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action) {
        foreach (var item in source) {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Sets the visibility and interactivity of the CanvasGroup.
    /// </summary>
    public static void Toggle(this CanvasGroup canvasGroup, bool isActive) {
        canvasGroup.alpha = isActive ? 1f : 0f;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }

    public static void FollowMouse(this Transform target) {
        var mousePosition = InputManager.Instance.GetMousePositionInWorld();
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        target.position = mousePosition;
    }

    //public static void FollowMouse(this RectTransform target) {
    //    Vector2 anchorPos;
    //    var canvas = UIManager.Instance.MainCanvas;
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        canvas.GetComponent<RectTransform>(),
    //        Input.mousePosition,
    //        canvas.worldCamera,
    //        out anchorPos);

    //    target.position = canvas.transform.TransformPoint(anchorPos);
    //}

    /// <summary>
    /// Rotate Up vector as Forward. 
    /// </summary>
    public static void FaceTo(this Transform from, Transform to, float step = 360f) {
        var direction = (to.position - from.position).normalized;
        var angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        from.rotation = Quaternion.RotateTowards(from.rotation, targetRotation, step);
    }

    /// <summary>
    /// Rotate Up vector as Forward. 
    /// </summary>
    public static void FaceTo(this Transform from, Vector3 direction, float step = 360f) {
        direction = direction.normalized;
        var angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        from.rotation = Quaternion.RotateTowards(from.rotation, targetRotation, step);
    }

    /// <summary>
    /// Find the closest target by distance. 
    /// </summary>
    public static Enemy FindClosestByDistance(this Vector3 position, float radius, LayerMask layerMask) {
        Enemy result = null;
        var closest = Mathf.Infinity;
        var colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
        foreach (var collider in colliders) {
            var obj = collider.GetComponent<Enemy>();
            var distance = Vector3.Distance(position, collider.ClosestPoint(position));
            if (distance < closest) {
                result = obj;
                closest = distance;
            }
        }
        return result;
    }

    public static List<Enemy> FindClosestByDistance(this Vector3 position, float radius, LayerMask layerMask, int num) {
        var result = new List<Enemy>();
        var distances = new List<float>();

        var farthestIndex = 0;
        var farthestDistance = Mathf.Infinity;

        var colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
        foreach (var collider in colliders) {
            var obj = collider.GetComponent<Enemy>();
            var distance = Vector3.Distance(position, collider.ClosestPoint(position));

            if (result.Count < num) {
                result.Add(obj);
                distances.Add(distance);
            }
            else if (distance < farthestDistance) {
                // replace
                result[farthestIndex] = obj;
                distances[farthestIndex] = distance;

                // update farthest
                for (int i = 0; i < result.Count; i++) {
                    if (distances[i] > farthestDistance) {
                        farthestDistance = distances[i];
                        farthestIndex = i;
                    }
                }
            }
        }
        return result;
    }

    public static List<Enemy> FindAllByDistance(this Vector3 position, float radius, LayerMask layerMask) {
        var result = new List<Enemy>();
        var colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
        foreach (var collider in colliders) {
            var obj = collider.GetComponent<Enemy>();
            result.Add(obj);
        }
        return result;
    }

    /// <summary>
    /// Find the closest target by angle. 
    /// </summary>
    public static Enemy FindClosestByAngle(this Vector3 position, Vector3 direction, float radius, LayerMask layerMask) {
        Enemy result = null;
        var closest = Mathf.Infinity;
        var colliders = Physics2D.OverlapCircleAll(position, radius, layerMask);
        foreach (var collider in colliders) {
            var enemy = collider.GetComponent<Enemy>();
            if (!enemy) continue;

            var angle = Vector3.Angle(direction, enemy.transform.position - position);
            if (angle < closest) {
                result = enemy;
                closest = angle;
            }
        }
        return result;
    }

    public static float DistanceTo(this Vector3 origin, Collider2D collider) {
        var closestPoint = collider.ClosestPoint(origin);
        var distance = Vector3.Distance(origin, closestPoint);
        return distance;
    }


#if UNITY_EDITOR
    public static List<T> FindRefsInFolder<T>(string folderPath, AssetType assetType = AssetType.All)
        where T : UnityEngine.Object {
        if (string.IsNullOrEmpty(folderPath)) {
            Debug.LogWarning("Path is empty. ");
            return new();
        }

        var filter = assetType switch {
            AssetType.Prefab => "t:Prefab",
            AssetType.ScriptableObject => "t:ScriptableObject",
            _ => "",
        };

        var guids = AssetDatabase.FindAssets(filter, new[] { folderPath });
        var objects = new List<T>();
        foreach (var guid in guids) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null) {
                objects.Add(asset);
            }
        }

        return objects;
    }
#endif
}

