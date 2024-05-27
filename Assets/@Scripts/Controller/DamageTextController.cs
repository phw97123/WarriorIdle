using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTextController : BaseController
{
    private TextMeshPro _text;
    public int Damage { get; set; }
    private float _moveSpeed = 1.5f;
    private float _destroyTime = 0.3f;

    public override bool Init()
    {
        base.Init();

        ObjectType = Define.ObjectType.DamageText;
        _text = GetComponent<TextMeshPro>();
        StartCoroutine(COFade());
        _text.color = Color.white;

        return true;
    }

    private void Update()
    {
        transform.Translate(new Vector3(0, _moveSpeed * Time.deltaTime, 0));
        _text.text = Damage.ToString();
    }

    private IEnumerator COFade()
    {
        _text.fontSize = 15;

        for (int i = (int)_text.fontSize; i >= 3; i--)
        {
            _text.fontSize = i;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(_destroyTime);
        Managers.ObjectManager.Despawn(this);
    }

    public void SetColor()
    {
        _text.color = Color.red; 
    }
}
