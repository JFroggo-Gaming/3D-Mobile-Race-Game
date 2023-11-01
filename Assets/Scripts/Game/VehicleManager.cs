using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleManager : MonoBehaviour
{   
    public static VehicleManager instance;
    public GameObject[] vehiclePrefabs; // Tablica z prefabami pojazdów.
    public Button[] vehicleButtons; // Tablica z przyciskami wyboru pojazdu.
    public CameraFollow cameraFollow; // Skrypt kamery
    public Transform vehicleSpawnPoint; // Punkt, gdzie pojazd będzie pojawiał się po wyborze.
    public GameObject uiPanel; // Panel UI z przyciskami wyboru pojazdu.

    public GameObject PlatformSpawner; // GameObject which starts generating platforms
    
    private int selectedVehicleIndex = -1; // Indeks wybranego pojazdu (-1 oznacza brak wyboru).

    public GameObject currentVehicle; // Obecnie używany pojazd.

    private void Start()
    {
        // Aktywuj obsługę przycisków wyboru pojazdu.
        for (int i = 0; i < vehicleButtons.Length; i++)
        {
            int vehicleIndex = i; // Tworzenie kopii indeksu do obsługi zdarzenia.

            // Dodaj obsługę zdarzenia kliknięcia na przycisk.
            vehicleButtons[i].onClick.AddListener(() =>
            {
                SelectVehicle(vehicleIndex);
            });
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void SelectVehicle(int vehicleIndex)
    {
        // Ustaw wybrany indeks pojazdu.
        selectedVehicleIndex = vehicleIndex;

        // Usuń poprzedni pojazd (jeśli istniał).
        if (currentVehicle != null)
        {
            Destroy(currentVehicle);
        }

        // Zainstancjuj nowy pojazd na scenie w określonym punkcie.
        currentVehicle = Instantiate(vehiclePrefabs[selectedVehicleIndex], vehicleSpawnPoint.position, vehicleSpawnPoint.rotation);

        // Przekaż wybrany pojazd do kamery do śledzenia.
        cameraFollow.SetTarget(currentVehicle.transform);

        GameManager.instance.ReloadLevelAfterSelect();

        // Wyłącz panel UI po wyborze pojazdu.
        uiPanel.SetActive(false);
        
    }
}
