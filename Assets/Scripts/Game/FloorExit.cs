using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour player))
        {
            // Pour le moment un simple chargement de scene, les améliorations pourront ensuite être ajoutées
            LoadNextFloor();
        }
    }

    public void LoadNextFloor()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
