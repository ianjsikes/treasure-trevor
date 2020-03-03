using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class Utils : ScriptableObject
{
  protected const string _newline = "\r\n";

  private static float sinH;

  public static Vector2 WorldToCanvasScreenPosition(Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
  {
    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : camera, out Vector2 localPoint);
    return canvas.transform.TransformPoint(localPoint);
  }

  public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
  {
    Vector2 result = camera.WorldToViewportPoint(position);
    result.x *= canvas.sizeDelta.x;
    result.y *= canvas.sizeDelta.y;
    result.x -= canvas.sizeDelta.x * canvas.pivot.x;
    result.y -= canvas.sizeDelta.y * canvas.pivot.y;
    return result;
  }

  public static float Clamp0360(float eulerAngles)
  {
    float num = eulerAngles - (float)Mathf.CeilToInt(eulerAngles / 360f) * 360f;
    if (num < 0f)
    {
      num += 360f;
    }
    return num;
  }

  public static bool IsApproximately(float _a, float _b, float _tolerance)
  {
    float num = _a - _b;
    return ((num < 0f) ? (0f - num) : num) < _tolerance;
  }

  public static Vector3 GetNormal(Vector3 _a, Vector3 _b, Vector3 _c)
  {
    Vector3 lhs = _b - _a;
    Vector3 rhs = _c - _a;
    return Vector3.Cross(lhs, rhs).normalized;
  }

  public static Vector3 LerpByDistance(Vector3 _a, Vector3 _b, float _x)
  {
    return _x * (_b - _a).normalized + _a;
  }

  public static bool IsVisibleFrom(Renderer _renderer, Camera _cam)
  {
    return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_cam), _renderer.bounds);
  }

  public static string WrapString(string _string, int _width)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (_width < 1)
    {
      return _string;
    }
    int i = 0;
    while (i < _string.Length)
    {
      int num = _string.IndexOf("\r\n", i);
      int num2 = (num != -1) ? (num + "\r\n".Length) : (num = _string.Length);
      if (num > i)
      {
        do
        {
          int num3 = num - i;
          if (num3 > _width)
          {
            num3 = BreakLine(_string, i, _width);
          }
          stringBuilder.Append(_string, i, num3);
          stringBuilder.Append("\r\n");
          for (i += num3; i < num && char.IsWhiteSpace(_string[i]); i++)
          {
          }
        }
        while (num > i);
      }
      else
      {
        stringBuilder.Append("\r\n");
      }
      i = num2;
    }
    return stringBuilder.ToString();
  }

  public static int BreakLine(string text, int pos, int max)
  {
    int num = max - 1;
    while (num >= 0 && !char.IsWhiteSpace(text[pos + num]))
    {
      num--;
    }
    if (num < 0)
    {
      return max;
    }
    while (num >= 0 && char.IsWhiteSpace(text[pos + num]))
    {
      num--;
    }
    return num + 1;
  }

  public static string PickRandomFromList(List<string> _list)
  {
    string text = "";
    int index = UnityEngine.Random.Range(1, _list.Count);
    text = _list[index];
    _list[index] = _list[0];
    _list[0] = text;
    return text;
  }

  public static AudioClip PickRandomAudioClipFromArray(AudioClip[] _array)
  {
    AudioClip audioClip = null;
    if (_array != null && _array.Length != 0)
    {
      if (_array.Length > 1)
      {
        int num = UnityEngine.Random.Range(1, _array.Length - 1);
        audioClip = _array[num];
        _array[num] = _array[0];
        _array[0] = audioClip;
      }
      else
      {
        audioClip = _array[0];
      }
    }
    return audioClip;
  }

  public static AudioClip PickRandomAudioClipFromList(List<AudioClip> _list)
  {
    AudioClip audioClip = null;
    if (_list != null)
    {
      if (_list.Count > 1)
      {
        int index = UnityEngine.Random.Range(1, _list.Count);
        audioClip = _list[index];
        _list[index] = _list[0];
        _list[0] = audioClip;
      }
      else
      {
        audioClip = _list[0];
      }
    }
    return audioClip;
  }

  public static Mesh PickRandomMeshFromArray(Mesh[] _array)
  {
    Mesh mesh = null;
    if (_array != null)
    {
      if (_array.Length > 1)
      {
        int num = UnityEngine.Random.Range(1, _array.Length - 1);
        mesh = _array[num];
        _array[num] = _array[0];
        _array[0] = mesh;
      }
      else
      {
        mesh = _array[0];
      }
    }
    return mesh;
  }

  public static Mesh PickRandomMeshFromList(List<Mesh> _list)
  {
    Mesh mesh = null;
    if (_list != null)
    {
      if (_list.Count > 1)
      {
        int index = UnityEngine.Random.Range(1, _list.Count);
        mesh = _list[index];
        _list[index] = _list[0];
        _list[0] = mesh;
      }
      else
      {
        mesh = _list[0];
      }
    }
    return mesh;
  }

  public static bool StartsWithVowel(string _checkString)
  {
    if (!_checkString.StartsWith("e") && !_checkString.StartsWith("a") && !_checkString.StartsWith("i") && !_checkString.StartsWith("o") && !_checkString.StartsWith("u") && !_checkString.StartsWith("E") && !_checkString.StartsWith("A") && !_checkString.StartsWith("I") && !_checkString.StartsWith("O"))
    {
      return _checkString.StartsWith("U");
    }
    return true;
  }

  public static void Shuffle<T>(T[] array, System.Random _rand)
  {
    int num = array.Length;
    for (int i = 0; i < num; i++)
    {
      int num2 = i + (int)(_rand.NextDouble() * (double)(num - i));
      T val = array[num2];
      array[num2] = array[i];
      array[i] = val;
    }
  }

  public static float ConvertRange(float _value, float _fromMin, float _fromMax, float _toMin, float _toMax)
  {
    _value = Mathf.Clamp(_value, _fromMin, _fromMax);
    float num = _fromMax - _fromMin;
    _value = (_value - _fromMin) / num;
    float num2 = _toMax - _toMin;
    _value = _toMin + _value * (num2 / 1f);
    return _value;
  }

  public static Texture2D TakeScreenshot(int _width, int _height, Camera _screenshotCamera)
  {
    if (_width <= 0 || _height <= 0)
    {
      return null;
    }
    if (_screenshotCamera == null)
    {
      _screenshotCamera = Camera.main;
    }
    Texture2D texture2D = new Texture2D(_width, _height, TextureFormat.RGB24, mipChain: false);
    RenderTexture renderTexture2 = _screenshotCamera.targetTexture = new RenderTexture(_width, _height, 24);
    _screenshotCamera.Render();
    RenderTexture.active = renderTexture2;
    texture2D.ReadPixels(new Rect(0f, 0f, _width, _height), 0, 0);
    texture2D.Apply(updateMipmaps: false);
    _screenshotCamera.targetTexture = null;
    RenderTexture.active = null;
    UnityEngine.Object.Destroy(renderTexture2);
    return texture2D;
  }

  public static string ColorToHex(Color32 _color, bool _includeAlpha)
  {
    string text = Convert.ToString(_color.r, 16).ToUpper();
    string text2 = Convert.ToString(_color.g, 16).ToUpper();
    string text3 = Convert.ToString(_color.b, 16).ToUpper();
    string text4 = Convert.ToString(_color.a, 16).ToUpper();
    while (text.Length < 2)
    {
      text = "0" + text;
    }
    while (text2.Length < 2)
    {
      text2 = "0" + text2;
    }
    while (text3.Length < 2)
    {
      text3 = "0" + text3;
    }
    while (text4.Length < 2)
    {
      text4 = "0" + text4;
    }
    if (_includeAlpha)
    {
      return "#" + text + text2 + text3 + text4;
    }
    return "#" + text + text2 + text3;
  }

  public static Color HexToColor(string _hex)
  {
    Color result = new Color(0f, 0f, 0f, 0f);
    if (_hex != null && _hex.Length > 0)
    {
      string text = _hex.Substring(1, _hex.Length - 1);
      result.r = (float)int.Parse(text.Substring(0, 2), NumberStyles.AllowHexSpecifier) / 255f;
      result.g = (float)int.Parse(text.Substring(2, 2), NumberStyles.AllowHexSpecifier) / 255f;
      result.b = (float)int.Parse(text.Substring(4, 2), NumberStyles.AllowHexSpecifier) / 255f;
      if (text.Length == 8)
      {
        result.a = (float)int.Parse(text.Substring(6, 2), NumberStyles.AllowHexSpecifier) / 255f;
      }
      else
      {
        result.a = 1f;
      }
    }
    return result;
  }

  public static Vector3 ColorToHSB(Color _color)
  {
    float num = Mathf.Min(_color.r, Mathf.Min(_color.g, _color.b));
    float num2 = Mathf.Max(_color.r, Mathf.Max(_color.g, _color.b));
    float num3 = num2 - num;
    float num4 = 0f;
    float z = num2;
    if (num2 == _color.r)
    {
      if (_color.g >= _color.b)
      {
        num4 = ((num3 != 0f) ? (60f * (_color.g - _color.b) / num3) : 0f);
      }
      else if (_color.g < _color.b)
      {
        num4 = 60f * (_color.g - _color.b) / num3 + 360f;
      }
    }
    else if (num2 == _color.g)
    {
      num4 = 60f * (_color.b - _color.r) / num3 + 120f;
    }
    else if (num2 == _color.b)
    {
      num4 = 60f * (_color.r - _color.g) / num3 + 240f;
    }
    return new Vector3(y: (num2 != 0f) ? (1f - num / num2) : 0f, x: num4 / 360f, z: z);
  }

  public static Color HSBToColor(Vector4 _hsba)
  {
    float r = _hsba.z;
    float g = _hsba.z;
    float b = _hsba.z;
    if (_hsba.y > 0f)
    {
      float num = _hsba.x * 360f / 60f;
      int num2 = Mathf.FloorToInt(num);
      float num3 = num - (float)num2;
      float num4 = _hsba.z * (1f - _hsba.y);
      float num5 = _hsba.z * (1f - _hsba.y * num3);
      float num6 = _hsba.z * (1f - _hsba.y * (1f - num3));
      switch (num2)
      {
        case 1:
          r = num5;
          g = _hsba.z;
          b = num4;
          break;
        case 2:
          r = num4;
          g = _hsba.z;
          b = num6;
          break;
        case 3:
          r = num4;
          g = num5;
          b = _hsba.z;
          break;
        case 4:
          r = num6;
          g = num4;
          b = _hsba.z;
          break;
        case 5:
          r = _hsba.z;
          g = num4;
          b = num5;
          break;
        default:
          r = _hsba.z;
          g = num6;
          b = num4;
          break;
      }
    }
    return new Color(r, g, b, _hsba.w);
  }

  public static float To360Anglefloat(float _angle)
  {
    while (_angle < 0f)
    {
      _angle += 360f;
    }
    while (_angle >= 360f)
    {
      _angle -= 360f;
    }
    return _angle;
  }

  public static Vector3 To360AngleVector3(Vector3 _angles)
  {
    _angles.x = To360Anglefloat(_angles.x);
    _angles.y = To360Anglefloat(_angles.y);
    _angles.z = To360Anglefloat(_angles.z);
    return _angles;
  }

  public static float To180Anglefloat(float _angle)
  {
    while (_angle < -180f)
    {
      _angle += 360f;
    }
    while (_angle >= 180f)
    {
      _angle -= 360f;
    }
    return _angle;
  }

  public static Vector3 To180AngleVector3(Vector3 _angles)
  {
    _angles.x = To180Anglefloat(_angles.x);
    _angles.y = To180Anglefloat(_angles.y);
    _angles.z = To180Anglefloat(_angles.z);
    return _angles;
  }

  public static float MathAngleToCompassAngle(float _angle)
  {
    _angle = 90f - _angle;
    return To360Anglefloat(_angle);
  }

  public static float CompassAngleLerp(float _from, float _to, float _portion)
  {
    float num = To180Anglefloat(_to - _from);
    num *= Mathf.Clamp01(_portion);
    return To360Anglefloat(_from + num);
  }

  public static Texture2D CreateEmptyTexture(int _w, int _h, Color _color)
  {
    Texture2D texture2D = new Texture2D(_w, _h, TextureFormat.RGBA32, mipChain: false);
    Color[] pixels = texture2D.GetPixels(0);
    for (int i = 0; i < pixels.Length; i++)
    {
      pixels[i] = _color;
    }
    texture2D.SetPixels(pixels, 0);
    texture2D.Apply();
    return texture2D;
  }

  public static Texture2D ChangeTextureColor(Texture2D _originalTexture, float _deltaHue, float _deltaSaturation, float _deltaBrightness)
  {
    Texture2D texture2D = new Texture2D(_originalTexture.width, _originalTexture.height, TextureFormat.RGBA32, mipChain: false);
    Color[] pixels = _originalTexture.GetPixels(0);
    Color[] pixels2 = texture2D.GetPixels(0);
    for (int i = 0; i < pixels.Length; i++)
    {
      Vector4 hsba = ColorToHSB(pixels[i]);
      hsba.x += _deltaHue;
      hsba.y += _deltaSaturation;
      hsba.z += _deltaBrightness;
      pixels2[i] = HSBToColor(hsba);
    }
    texture2D.SetPixels(pixels2, 0);
    texture2D.Apply();
    return texture2D;
  }

  public static Texture2D ChangeTextureContrastLinear(Texture2D _originalTexture, float _contrast, float _power)
  {
    if (_power < 0f)
    {
      _power = 1f;
    }
    Texture2D texture2D = new Texture2D(_originalTexture.width, _originalTexture.height, TextureFormat.RGBA32, mipChain: false);
    Color[] pixels = _originalTexture.GetPixels(0);
    Color[] pixels2 = texture2D.GetPixels(0);
    float num = 0f;
    for (int i = 0; i < pixels.Length; i++)
    {
      Color color = pixels[i];
      num += color.r;
      num += color.g;
      num += color.b;
    }
    num /= 3f * (float)pixels.Length;
    for (int j = 0; j < pixels.Length; j++)
    {
      Color color2 = pixels[j];
      float num2 = color2.r - num;
      float num3 = color2.g - num;
      float num4 = color2.b - num;
      pixels2[j] = new Color(num + num2 * _contrast, num + num3 * _contrast, num + num4 * _contrast, color2.a);
    }
    texture2D.SetPixels(pixels2, 0);
    texture2D.Apply();
    return texture2D;
  }

  public static Texture2D CropTexture(Texture2D _originalTexture, Rect _cropRect)
  {
    _cropRect.x = Mathf.Clamp(_cropRect.x, 0f, _originalTexture.width);
    _cropRect.width = Mathf.Clamp(_cropRect.width, 0f, (float)_originalTexture.width - _cropRect.x);
    _cropRect.y = Mathf.Clamp(_cropRect.y, 0f, _originalTexture.height);
    _cropRect.height = Mathf.Clamp(_cropRect.height, 0f, (float)_originalTexture.height - _cropRect.y);
    if (_cropRect.height <= 0f || _cropRect.width <= 0f)
    {
      return null;
    }
    Texture2D texture2D = new Texture2D((int)_cropRect.width, (int)_cropRect.height, TextureFormat.RGBA32, mipChain: false);
    Color[] pixels = _originalTexture.GetPixels((int)_cropRect.x, (int)_cropRect.y, (int)_cropRect.width, (int)_cropRect.height, 0);
    texture2D.SetPixels(pixels);
    texture2D.Apply();
    return texture2D;
  }

  public static Texture2D MirrorTexture(Texture2D _originalTexture, bool _horizontal, bool _vertical)
  {
    Texture2D texture2D = new Texture2D(_originalTexture.width, _originalTexture.height, TextureFormat.RGBA32, mipChain: false);
    Color[] pixels = _originalTexture.GetPixels(0);
    Color[] pixels2 = texture2D.GetPixels(0);
    for (int i = 0; i < _originalTexture.height; i++)
    {
      for (int j = 0; j < _originalTexture.width; j++)
      {
        int num = _horizontal ? (texture2D.width - 1 - j) : j;
        int num2 = _vertical ? (texture2D.height - 1 - i) : i;
        pixels2[num2 * texture2D.width + num] = pixels[i * _originalTexture.width + j];
      }
    }
    texture2D.SetPixels(pixels2, 0);
    texture2D.Apply();
    return texture2D;
  }

  public static int StringToInt(string _string)
  {
    int result = 0;
    if (_string != null && int.TryParse(_string, out result))
    {
      return result;
    }
    return 0;
  }

  public static float StringTofloat(string _string)
  {
    float result = 0f;
    if (_string != null && float.TryParse(_string, out result))
    {
      return result;
    }
    return 0f;
  }

  public static Vector3 StringToVector3(string _string)
  {
    Vector3 result = new Vector3(0f, 0f, 0f);
    if (_string != null && _string.Length > 0 && _string.IndexOf(",", 0) >= 0)
    {
      int num = 0;
      int num2 = 0;
      int num3 = 0;
      num2 = _string.IndexOf(",", num);
      while (num2 > num && num3 <= 3)
      {
        result[num3++] = float.Parse(_string.Substring(num, num2 - num));
        num = num2 + 1;
        if (num < _string.Length)
        {
          num2 = _string.IndexOf(",", num);
        }
        if (num2 < 0)
        {
          num2 = _string.Length;
        }
      }
    }
    return result;
  }

  public static string floatToString(float _float, int _decimals)
  {
    if (_decimals <= 0)
    {
      return string.Concat(Mathf.RoundToInt(_float));
    }
    return string.Format("{0:F" + _decimals + "}", _float);
  }

  public static string Vector3ToString(Vector3 _vector, int _decimals)
  {
    if (_decimals <= 0)
    {
      return "<" + Mathf.RoundToInt(_vector.x) + "," + Mathf.RoundToInt(_vector.y) + "," + Mathf.RoundToInt(_vector.z) + ">";
    }
    string format = "{0:F" + _decimals + "}";
    return "<" + string.Format(format, _vector.x) + "," + string.Format(format, _vector.y) + "," + string.Format(format, _vector.z) + ">";
  }

  public static Vector3 PickPointWithinRadius(Vector3 _origin, float _rMin, float _rMax)
  {
    Vector3 result = _origin + UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(_rMin, _rMax);
    result.y = _origin.y;
    return result;
  }

  public static void RotateTowardsPoint(Transform _applyTo, Vector3 _from, Vector3 _to, float _speed, bool _freeRotation, Vector3 _extraOffset)
  {
    Vector3 forward = _to - _from;
    forward.y = _from.y;
    Quaternion b = Quaternion.LookRotation(forward) * Quaternion.Euler(_extraOffset);
    if (!_freeRotation)
    {
      b.x = 0f;
      b.z = 0f;
    }
    _applyTo.rotation = Quaternion.Slerp(_applyTo.rotation, b, _speed);
  }

  public static void RotateTowardsPointComplex(Transform _applyTo, Vector3 _from, Vector3 _to, float _speed, bool _freeRotation, Vector3 _extraOffset)
  {
    Quaternion b = Quaternion.LookRotation(_to - _from) * Quaternion.Euler(_extraOffset);
    if (!_freeRotation)
    {
      b.x = 0f;
      b.z = 0f;
    }
    _applyTo.localRotation = Quaternion.Slerp(_applyTo.localRotation, b, _speed);
  }

  public static Vector3[] MakeSmoothCurve(Vector3[] _arrayToCurve, float _smoothness)
  {
    int num = 0;
    int num2 = 0;
    _smoothness = Mathf.Clamp(_smoothness, 1f, 10f);
    num = _arrayToCurve.Length;
    num2 = num * Mathf.RoundToInt(_smoothness) - 1;
    List<Vector3> list = new List<Vector3>(num2);
    float num3 = 0f;
    for (int i = 0; i < num2 + 1; i++)
    {
      num3 = Mathf.InverseLerp(0f, num2, i);
      List<Vector3> list2 = new List<Vector3>(_arrayToCurve);
      for (int num4 = num - 1; num4 > 0; num4--)
      {
        for (int j = 0; j < num4; j++)
        {
          list2[j] = (1f - num3) * list2[j] + num3 * list2[j + 1];
        }
      }
      list.Add(list2[0]);
    }
    return list.ToArray();
  }

  public static void AddDescendantsWithMeshRenderer(Transform _parent, ref List<MeshRenderer> _list)
  {
    foreach (Transform item in _parent)
    {
      if (item.GetComponent<MeshRenderer>() != null)
      {
        MeshRenderer component = item.GetComponent<MeshRenderer>();
        if (component != null)
        {
          _list.Add(component);
        }
      }
      AddDescendantsWithMeshRenderer(item, ref _list);
    }
  }

  public static List<Vector3> MakeSmoothCurveList(List<Vector3> _listToCurve, float _smoothness)
  {
    int num = 0;
    int num2 = 0;
    _smoothness = Mathf.Clamp(_smoothness, 1f, 10f);
    num = _listToCurve.Count;
    num2 = num * Mathf.RoundToInt(_smoothness) - 1;
    List<Vector3> list = new List<Vector3>(num2);
    float num3 = 0f;
    for (int i = 0; i < num2 + 1; i++)
    {
      num3 = Mathf.InverseLerp(0f, num2, i);
      List<Vector3> list2 = new List<Vector3>(_listToCurve);
      for (int num4 = num - 1; num4 > 0; num4--)
      {
        for (int j = 0; j < num4; j++)
        {
          list2[j] = (1f - num3) * list2[j] + num3 * list2[j + 1];
        }
      }
      list.Add(list2[0]);
    }
    return list;
  }

  public static Vector3[] Chaikin(Vector3[] _pts)
  {
    Vector3[] array = new Vector3[0];
    if (_pts != null && _pts.Length != 0)
    {
      array = new Vector3[(_pts.Length - 2) * 2 + 2];
      array[0] = _pts[0];
      array[array.Length - 1] = _pts[_pts.Length - 1];
      int num = 1;
      for (int i = 0; i < _pts.Length - 2; i++)
      {
        array[num] = _pts[i] + (_pts[i + 1] - _pts[i]) * 0.75f;
        array[num + 1] = _pts[i + 1] + (_pts[i + 2] - _pts[i + 1]) * 0.25f;
        num += 2;
      }
    }
    return array;
  }

  public static bool IsEven(int _value)
  {
    return _value % 2 == 0;
  }

  public static bool IsPointInside(Mesh _mesh, Vector3 _localPoint)
  {
    Vector3[] vertices = _mesh.vertices;
    int[] triangles = _mesh.triangles;
    int num = triangles.Length / 3;
    for (int i = 0; i < num; i++)
    {
      Vector3 a = vertices[triangles[i * 3]];
      Vector3 b = vertices[triangles[i * 3 + 1]];
      Vector3 c = vertices[triangles[i * 3 + 2]];
      if (new Plane(a, b, c).GetSide(_localPoint))
      {
        return false;
      }
    }
    return true;
  }

  public static float AngleInRad(Vector3 _v1, Vector3 _v2)
  {
    return Mathf.Atan2(_v2.y - _v1.y, _v2.x - _v1.x);
  }

  public static float AngleInDegrees(Vector3 _v1, Vector3 _v2)
  {
    return AngleInRad(_v1, _v2) * 180f / (float)Math.PI;
  }

  public static void ResetTransform(Transform _t)
  {
    if (_t != null)
    {
      _t.localPosition = Vector3.zero;
      _t.localScale = Vector3.one;
      _t.localRotation = Quaternion.identity;
    }
  }

  public static float SinLP(float _x)
  {
    if (_x < -(float)Math.PI)
    {
      _x += (float)Math.PI * 2f;
    }
    else if (_x > (float)Math.PI)
    {
      _x -= (float)Math.PI * 2f;
    }
    if (_x < 0f)
    {
      return _x * (4f / (float)Math.PI + 0.40528473f * _x);
    }
    return _x * (4f / (float)Math.PI - 0.40528473f * _x);
  }

  public static float SinHP(float _x)
  {
    if (_x < -(float)Math.PI)
    {
      _x += (float)Math.PI * 2f;
    }
    else if (_x > (float)Math.PI)
    {
      _x -= (float)Math.PI * 2f;
    }
    if (_x < 0f)
    {
      sinH = _x * (4f / (float)Math.PI + 0.40528473f * _x);
      if (sinH < 0f)
      {
        sinH *= -0.255f * (sinH + 1f) + 1f;
      }
      else
      {
        sinH *= 0.255f * (sinH - 1f) + 1f;
      }
    }
    else
    {
      sinH = _x * (4f / (float)Math.PI - 0.40528473f * _x);
      if (sinH < 0f)
      {
        sinH *= -0.255f * (sinH + 1f) + 1f;
      }
      else
      {
        sinH *= 0.255f * (sinH - 1f) + 1f;
      }
    }
    return sinH;
  }

  public static GameObject GenerateCone(Vector3 _pos, Quaternion _rot, Vector3 _scale, float _height, float _bottomRadius, float _topRadius)
  {
    GameObject gameObject = new GameObject("proceduralCone");
    Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;
    mesh.Clear();
    int num = 32;
    int num2 = 1;
    int num3 = num + 1;
    Vector3[] array = new Vector3[num3 + num3 + num * num2 * 2 + 2];
    int i = 0;
    float num4 = (float)Math.PI * 2f;
    array[i++] = Vector3.zero;
    for (; i <= num; i++)
    {
      float f = (float)i / (float)num * num4;
      array[i] = new Vector3(Mathf.Cos(f) * _bottomRadius, 0f, Mathf.Sin(f) * _bottomRadius);
    }
    array[i++] = new Vector3(0f, _height, 0f);
    for (; i <= num * 2 + 1; i++)
    {
      float f2 = (float)(i - num - 1) / (float)num * num4;
      array[i] = new Vector3(Mathf.Cos(f2) * _topRadius, _height, Mathf.Sin(f2) * _topRadius);
    }
    int num5 = 0;
    while (i <= array.Length - 4)
    {
      float f3 = (float)num5 / (float)num * num4;
      array[i] = new Vector3(Mathf.Cos(f3) * _topRadius, _height, Mathf.Sin(f3) * _topRadius);
      array[i + 1] = new Vector3(Mathf.Cos(f3) * _bottomRadius, 0f, Mathf.Sin(f3) * _bottomRadius);
      i += 2;
      num5++;
    }
    array[i] = array[num * 2 + 2];
    array[i + 1] = array[num * 2 + 3];
    Vector3[] array2 = new Vector3[array.Length];
    i = 0;
    while (i <= num)
    {
      array2[i++] = Vector3.down;
    }
    while (i <= num * 2 + 1)
    {
      array2[i++] = Vector3.up;
    }
    num5 = 0;
    while (i <= array.Length - 4)
    {
      float f4 = (float)num5 / (float)num * num4;
      float x = Mathf.Cos(f4);
      float z = Mathf.Sin(f4);
      array2[i] = new Vector3(x, 0f, z);
      array2[i + 1] = array2[i];
      i += 2;
      num5++;
    }
    array2[i] = array2[num * 2 + 2];
    array2[i + 1] = array2[num * 2 + 3];
    Vector2[] array3 = new Vector2[array.Length];
    int j = 0;
    array3[j++] = new Vector2(0.5f, 0.5f);
    for (; j <= num; j++)
    {
      float f5 = (float)j / (float)num * num4;
      array3[j] = new Vector2(Mathf.Cos(f5) * 0.5f + 0.5f, Mathf.Sin(f5) * 0.5f + 0.5f);
    }
    array3[j++] = new Vector2(0.5f, 0.5f);
    for (; j <= num * 2 + 1; j++)
    {
      float f6 = (float)j / (float)num * num4;
      array3[j] = new Vector2(Mathf.Cos(f6) * 0.5f + 0.5f, Mathf.Sin(f6) * 0.5f + 0.5f);
    }
    int num6 = 0;
    while (j <= array3.Length - 4)
    {
      float x2 = (float)num6 / (float)num;
      array3[j] = new Vector3(x2, 1f);
      array3[j + 1] = new Vector3(x2, 0f);
      j += 2;
      num6++;
    }
    array3[j] = new Vector2(1f, 1f);
    array3[j + 1] = new Vector2(1f, 0f);
    int num7 = num + num + num * 2;
    int[] array4 = new int[num7 * 3 + 3];
    int num8 = 0;
    int num9 = 0;
    while (num8 < num - 1)
    {
      array4[num9] = 0;
      array4[num9 + 1] = num8 + 1;
      array4[num9 + 2] = num8 + 2;
      num8++;
      num9 += 3;
    }
    array4[num9] = 0;
    array4[num9 + 1] = num8 + 1;
    array4[num9 + 2] = 1;
    num8++;
    num9 += 3;
    while (num8 < num * 2)
    {
      array4[num9] = num8 + 2;
      array4[num9 + 1] = num8 + 1;
      array4[num9 + 2] = num3;
      num8++;
      num9 += 3;
    }
    array4[num9] = num3 + 1;
    array4[num9 + 1] = num8 + 1;
    array4[num9 + 2] = num3;
    num8++;
    num9 += 3;
    num8++;
    while (num8 <= num7)
    {
      array4[num9] = num8 + 2;
      array4[num9 + 1] = num8 + 1;
      array4[num9 + 2] = num8;
      num8++;
      num9 += 3;
      array4[num9] = num8 + 1;
      array4[num9 + 1] = num8 + 2;
      array4[num9 + 2] = num8;
      num8++;
      num9 += 3;
    }
    mesh.vertices = array;
    mesh.normals = array2;
    mesh.uv = array3;
    mesh.triangles = array4;
    mesh.RecalculateBounds();
    Transform transform = gameObject.transform;
    transform.SetPositionAndRotation(_pos, _rot);
    transform.localScale = _scale;
    return gameObject;
  }
}
