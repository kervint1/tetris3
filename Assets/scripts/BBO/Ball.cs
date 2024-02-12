using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float speed;
    private Vector2 m_direction = new(1, 1);

    Rigidbody2D m_Rigidbody;
    Vector2 m_velocity;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        
        m_direction.Normalize();

        m_velocity = m_direction *  Time.deltaTime * speed;
        m_Rigidbody.velocity = m_velocity;
        Debug.Log("ballstart");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //.contacts[0]��0�Ԗڂ̐ڐG�_�̃f�[�^�擾
        //.normal�Ŗ@���x�N�g���擾
        var inDirection = m_velocity;
        var inNormal = collision.contacts[0].normal;

        m_direction = Vector2.Reflect(inDirection, inNormal).normalized;

        m_velocity = m_direction * Time.deltaTime * speed;
        m_Rigidbody.velocity = m_velocity;
    }
}
