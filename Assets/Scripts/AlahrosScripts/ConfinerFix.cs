
using Unity.Cinemachine;
using UnityEngine;
public class ConfinerUpdater : MonoBehaviour
{
    public CinemachineConfiner2D cinemachineConfiner;
    void UpdateConfiner() { if (cinemachineConfiner != null) { cinemachineConfiner.InvalidateLensCache(); } }
}