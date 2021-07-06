using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControll : MonoBehaviour
{
    public Camera mainCam;
    private Vector3 camPos;//Позиция камеры
    private Quaternion rotate/*Поворот корабля*/, camRotate;//Поворот камеры
    private float speed = 0/*Скорость самолета*/, speedNew/*Новая скорость самолета*/, speedStatic = 0.2f;//Статичная скорость самолета, от которой зависит все предыдущая
    public float _crystalls;//Колличество кристаллов
    private Vector3 rotateSpeed;//Скорость изменения поворота камеры, типо velocity для поворота

    public GameObject[] fills;//Палочки индикатора кристаллов
    private bool death;//Смерть? Ну вроде понятно
    public Renderer render;
    public GameObject deathParticle, statsParticle, statsParticle2, statsParticle3, restartButt;//Самые разные партиклы
    public Animation statsAnim;//Анимация индикатора
    public Animator restartAnim;//Аниматор кнопки рестарта
    public float crystalls
    {
        get { return _crystalls; }
        set
        {
            _crystalls = value;
            for(int i = 0; i < 5; i++)//Ставит индикатор под колличество кристаллов
            {
                fills[i].SetActive(false);
                if(i + 1 <= crystalls)
                {
                    fills[i].SetActive(true);
                }
            }
            if (crystalls >= 5)//Если кристаллов макс. то запускает корутин для апгрейда
            {
                StartCoroutine(StatsUp());
            }
        }
    }

    public IEnumerator StatsUp()//Включение эффектов  и проигрывания анимаций апгрейда
    {
        crystalls -= 5;
        statsAnim.Play();
        statsParticle.SetActive(true);
        statsParticle3.SetActive(true);
        yield return new WaitForSeconds(2);
        statsParticle2.SetActive(true);
        yield return new WaitForSeconds(5f);
        speedStatic += 0.1f;
        yield return new WaitForSeconds(3f);
        statsParticle.SetActive(false);
        statsParticle2.SetActive(false);
        statsParticle3.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!death) 
        {
            if (Input.GetKey(KeyCode.LeftShift))//Типо ускорение
            {
                speedNew = speedStatic * 2;
            }
            else
            {
                speedNew = speedStatic;
            }
            speed = Mathf.Lerp(speed, speedNew, 0.1f);//Плавно изменяет скорость на новую
            transform.position = transform.position + (transform.forward * speed);//Двигает самолет вперед

            if (transform.position.y >= 140)//Если игрок высоко взлетает, то направляет самолет вниз
            {
                rotateSpeed.x = Mathf.Lerp(rotateSpeed.x, 0.5f, 0.3f);
            }

            if (Input.GetKey(KeyCode.A))//Ну тут мне лень столько расписывать, сам разберись
            {
                rotateSpeed.y = Mathf.Lerp(rotateSpeed.y, -0.5f, 0.05f);
                rotate.z = Mathf.Lerp(rotate.z, 30, 0.02f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rotateSpeed.y = Mathf.Lerp(rotateSpeed.y, 0.5f, 0.05f);
                rotate.z = Mathf.Lerp(rotate.z, -30, 0.02f);
            }
            else
            {
                rotate.z = Mathf.Lerp(rotate.z, 0, 0.02f);
                rotateSpeed.y = Mathf.Lerp(rotateSpeed.y, 0, 0.02f);
            }
            rotate.y += rotateSpeed.y;

            if (Input.GetKey(KeyCode.W) && transform.position.y < 140)
            {
                rotateSpeed.x = Mathf.Lerp(rotateSpeed.x, -0.5f, 0.05f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rotateSpeed.x = Mathf.Lerp(rotateSpeed.x, 0.5f, 0.05f);
            }
            else
            {
                rotateSpeed.x = Mathf.Lerp(rotateSpeed.x, 0f, 0.05f);
            }
            rotate.x += rotateSpeed.x;

            transform.rotation = Quaternion.Euler(rotate.x, rotate.y, rotate.z); 
        }

        if (!death)
        {
            //Выставляет позицию камере
            camPos = transform.position;
            camPos += transform.forward * -5;
            camPos.y += 2;

            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, camPos, 0.1f);
        }
        //Выставляет поворот камеры
        mainCam.transform.LookAt(transform);
        Quaternion newRotate = Quaternion.Euler(mainCam.transform.eulerAngles.x, mainCam.transform.eulerAngles.y, mainCam.transform.eulerAngles.z);
        //camRotate = Vector3.Lerp(camRotate, newRotate, 0.1f);
        camRotate = Quaternion.Lerp(camRotate, newRotate, 0.1f);
        mainCam.transform.rotation = camRotate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)//Подбор кристалла и удаление его
        {
            Destroy(collision.gameObject);
            crystalls++;
        }
        else if(collision.gameObject.layer == 9)//Самолет врезался
        {
            death = true;
            render.enabled = false;
            deathParticle.SetActive(true);
            restartButt.SetActive(true);
        }
    }

    public void Restart()//Этот метод запускается кнопкой рестарта, включает анимацию кнопки а затем рестарт сцены
    {
        restartAnim.SetBool("R", true);
        Invoke("Restart2", 0.75f);
    }
    public void Restart2()
    {
        SceneManager.LoadScene(0);
    }
}
