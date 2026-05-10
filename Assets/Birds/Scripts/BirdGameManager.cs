using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdGameManager : MonoBehaviour
{
    private List<Target> targets;

    [SerializeField]
    private GameObject winScreen;

    private void Start()
    {
        targets = FindObjectsByType<Target>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
            .ToList();
        foreach (Target target in targets)
        {
            target.OnTargetDestoryed += OnTargetDestroyed;
        }
    }

    private void OnTargetDestroyed(Target target)
    {
        target.OnTargetDestoryed -= OnTargetDestroyed;
        targets.Remove(target);
        if (targets.Count == 0)
        {
            print("All targets destroyed");
            winScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
