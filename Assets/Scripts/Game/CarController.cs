using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float MoveSpeed;
    bool movingLeft = true;
    bool firstInput = true;

    private float originalSpeed; // Początkowa prędkość samochodu
    private float currentSpeed; // Aktualna prędkość samochodu
    private bool speedingUp; // Czy samochód przyspiesza

    // Zdarzenie, które zostanie wywołane, gdy samochód wejdzie w obszar obiektu z tagiem "Star"
    public delegate void StarCollected();
    public static event StarCollected OnStarCollected;

    void Start()
    {
        originalSpeed = MoveSpeed; // Zapisz początkową prędkość
    }

    void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            Move();
            CheckInput();
        }

        // checking if the car is outside the platform - so game over can be triggered
        if (transform.position.y <= -2)
        {
            GameManager.instance.GameOver();
        }

        if (speedingUp)
        {
            currentSpeed -= Time.deltaTime * 4f; // 4f to szybkość spadku prędkości
            MoveSpeed = currentSpeed;

            if (currentSpeed <= originalSpeed)
            {
                speedingUp = false;
                MoveSpeed = originalSpeed; // Przywróć początkową prędkość
            }
        }
    }

    void Move()
    {
        // Przesunięcie samochodu zgodnie z prędkością
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
    }

    void CheckInput()
    {
        if (firstInput)
        {
            firstInput = false;
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        if (movingLeft)
        {
            movingLeft = false;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            movingLeft = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // Funkcja do przyspieszania samochodu
    public void SpeedUp(float speedBoost)
    {
        speedingUp = true;
        currentSpeed = MoveSpeed + speedBoost;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            // Przyspiesz samochód o 5 jednostek na 5 sekund
            SpeedUp(5f);

            // Usuń obiekt "Star"
            Destroy(other.gameObject);

            // Opcjonalnie: Wywołaj zdarzenie OnStarCollected
            if (OnStarCollected != null)
            {
                OnStarCollected();
            }
        }
    }
}
