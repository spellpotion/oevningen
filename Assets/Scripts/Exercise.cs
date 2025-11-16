using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Exercise : MonoBehaviour
{
    private List<VisualElement> fingerPositions = new();

    private Coroutine showLesson;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        for (var i = 0; i < 12; i++)
        {
            var fingerPosition = root.Q<VisualElement>($"finger-position-{i}");

            fingerPositions.Add(fingerPosition);
        }

        var touchBoard = root.Q<VisualElement>($"touchboard");
        touchBoard.RegisterCallback<ClickEvent>((evt) =>
        {
            if (showLesson != null)
            {
                StopCoroutine(showLesson);

                foreach (var fingerPosition in fingerPositions)
                {
                    fingerPosition.RemoveFromClassList("first");
                    fingerPosition.RemoveFromClassList("pressable");
                    fingerPosition.RemoveFromClassList("press");
                }
            }

            var lesson = RandomLesson();

            fingerPositions[lesson[0]].AddToClassList("first");
            foreach (var index in lesson)
            {
                fingerPositions[index].AddToClassList("pressable");
            }

            showLesson = StartCoroutine(ShowLesson務(lesson));
        });
    }

    private IEnumerator ShowLesson務(List<int> lesson)
    {
        var index = 0;

        while (true)
        {
            fingerPositions[lesson[index]].AddToClassList("press");

            yield return new WaitForSeconds(.55f);

            fingerPositions[lesson[index]].RemoveFromClassList("press");

            index = ++index % lesson.Count;
        }
    }

    private static List<int> RandomLesson()
    {
        var valuesY = new List<int> { 0, 1, 2, 3, 4, 5 };

        var valuesX0 = valuesY.OrderBy(_ => Random.value).Take(2).ToList();
        var valuesX1 = valuesY.OrderBy(_ => Random.value).Take(2).ToList();

        var valuesAll = new List<(int x, int y)>
        {
            (0, valuesX0[0]),
            (0, valuesX0[1]),
            (1, valuesX1[0]),
            (1, valuesX1[1]),
        };
        valuesAll = valuesAll.OrderBy(_ => Random.value).ToList();

        return valuesAll.Select(valuesAll => valuesAll.x == 0 ? valuesAll.y : valuesAll.y + 6).ToList();
    }


}
