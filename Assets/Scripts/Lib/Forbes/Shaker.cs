using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShakeCfg
{
    public float Amplitude;
    public float Duration;
    public Vector3 Origin;
    public Vector3 Listener;
    public Transform ShakeWith;
    public float dA;
}

public class Shaker : MonoBehaviour
{
    public Transform targetTransform;
    Vector3 initPosition;
    float minAmplitude = 0.001f;
    ShakeCfg shake;

    public Shaker()
    {
        shake = new ShakeCfg();
    }

    void FixedUpdate()
    {
        Shake();
    }

    public void Shake()
    {
        if (targetTransform == null || shake == null)
            return;

        if (Time.time >= shake.Duration || shake.Amplitude <= minAmplitude)
        {
            targetTransform.position = initPosition;
            return;
        }

        //Debug.Log("Shaker shaking " + shake.Amplitude + " T: " + targetTransform);

        shake.Amplitude -= shake.dA;

        float randX = Random.Range(-shake.Amplitude, shake.Amplitude);
        float randY = Random.Range(-shake.Amplitude, shake.Amplitude);

        Vector3 shakePosition = new Vector3(randX, randY, 0);
        targetTransform.position = initPosition + shakePosition;
    }

    public void Set(ShakeCfg _cfg)
    {
        if (_cfg == null || _cfg.ShakeWith == null)
            return;

        initPosition = _cfg.ShakeWith.position;
        targetTransform = _cfg.ShakeWith;

        float distance = Vector3.Distance(_cfg.Origin, _cfg.Listener);
        if (distance >= 1f)
            _cfg.Amplitude /= distance * distance;

        _cfg.dA = _cfg.Amplitude / _cfg.Duration * Time.fixedDeltaTime;
        _cfg.Duration += Time.time;
        shake = _cfg;

        //targetTransform.position = initPosition;
        // Debug.Log("A/D/dA/d");
        // Debug.Log(shake.Amplitude);
        // Debug.Log(_cfg.Duration);
        // Debug.Log(shake.dA);
        // Debug.Log(distance);
    }
}