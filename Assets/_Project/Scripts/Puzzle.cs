using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public Transform emptyCell;
    public List<PuzzleTile> tiles;
    public List<Vector3> positions = new();
    public UnityEvent onWin;
    bool isStarted = false;
    bool isWin = false;

    private void Start()
    {
        GameEvents.Instance.onTileMove += CheckIfCanMove;

        foreach (var pos in tiles)
        {
            positions.Add(pos.correctPosition);
        }
        positions.Add(emptyCell.localPosition);

        Shuffle();
    }

    private void Update()
    {
        if (isStarted)
            if (!tiles.Any(x => x.transform.localPosition != x.correctPosition) && !isWin)
            {
                onWin?.Invoke();
                isWin = true;
            }
    }

    void Shuffle()
    {
        List<Transform> tilesTransforms = new List<Transform>();
        tiles.ForEach(x => tilesTransforms.Add(x.transform));
        tilesTransforms.Add(emptyCell);

        foreach (var pos in positions)
        {
            var item = tilesTransforms[Random.Range(0, tilesTransforms.Count - 1)];
            item.transform.localPosition = new Vector3(pos.x, pos.y, pos.z);
            tilesTransforms.Remove(item);
        }
        isStarted = true;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onTileMove -= CheckIfCanMove;
    }

    private void CheckIfCanMove(Transform obj)
    {
        if (Vector3.Distance(emptyCell.localPosition, obj.localPosition) <= 1.1f)
        {
            Vector3 tempPos = emptyCell.localPosition;
            emptyCell.localPosition = obj.localPosition;
            obj.localPosition = tempPos;
        }
    }

    public void Resolve()
    {
        foreach (var item in tiles)
        {
            item.transform.localPosition = item.correctPosition;
        }
    }
}
