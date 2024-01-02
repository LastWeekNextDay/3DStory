using System.Collections;
using UnityEngine;

public class SeeThroughWallSync : MonoBehaviour
{
    public static int PosID = Shader.PropertyToID("_ScreenPos");
    public static int OpacityID = Shader.PropertyToID("_Opacity");
    private Material _material;
    private Camera _camera;
    public LayerMask layerMask;

    private bool _fadingUp = false;
    private bool _fadingDown = false;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        var dir = _camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);
        if (Physics.Raycast(ray, out var hitInfo, 10000, layerMask))
        {
            _material = hitInfo.collider.GetComponent<Renderer>().material;
            if (_fadingDown){
                StopCoroutine("FadeTo");
                _fadingDown = false;
            }
            if (_fadingUp == false){
                _fadingUp = true;
                StartCoroutine("FadeTo", 0.5); 
            }
        }
        else
        {
            if (_fadingUp){
                StopCoroutine("FadeTo");
                _fadingUp = false;
            }
            if (_fadingDown == false){
                _fadingDown = true;
                StartCoroutine("FadeTo", 0);
            }
        }
        
        if (_material != null)
        {
            var position = transform.position + Vector3.up * GetComponent<Collider>().bounds.size.y/2;
            var view = _camera.WorldToViewportPoint(position);
            _material.SetVector(PosID, view);
        }
    }

    private IEnumerator FadeTo(float aValue)
    {
        if (_material == null) yield break;
        float alpha = _material.GetFloat(OpacityID);
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 0.2f)
        {
            _material.SetFloat(OpacityID, Mathf.Lerp(alpha, aValue, t));
            yield return null;
        }
    }
}
