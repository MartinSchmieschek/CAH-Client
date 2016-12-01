using UnityEngine;
using System.Collections;
using System;

namespace Assets.Controller.Phase
{

    public class Selector : Atom
    {
        public Atom ScreenSaverPhase;
        public float UpdateTiming = 0.1f;
        public float MaxStayOnTime = 10f;
        public bool MouseInteraction = true;
        public bool KeyBoardInteraction = true;
        public KeyCode NextKey = KeyCode.UpArrow;
        public KeyCode PreviewsKey = KeyCode.DownArrow;

        [SerializeField]
        public SelectorItem[] SelectableItems;

        private int selectionIndex = 0;
        private int lastSelection = 0;
        private float stayontime;

        public void Awake()
        {
            doSelection();
        }

        public Selector()
        {
            stayontime = 0f;
            ScreenSaverPhase = null;
        }

        private void MouseInput()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    for (int i = 0; i < SelectableItems.Length; i++)
                    {
                        if (SelectableItems[i].MouseCollider != null && SelectableItems[i].MouseCollider.Equals(hit.collider))
                        {
                            SelectByID(i);

                            if (Input.GetMouseButtonDown(0))
                                Activate();
                        }
                    }
                }
            }
        }

        private void GetKeyBoardInput()
        {

            if (Input.GetKeyDown(NextKey))
                SelectNext();

            if (Input.GetKeyDown(PreviewsKey))
                SelectLast();
        }

        public void FixedUpdate()
        {
            // MouseUpdate
            if (MouseInteraction)
                MouseInput();

            // keyboardInput
            if (KeyBoardInteraction)
                GetKeyBoardInput();
        }

        public override IEnumerator PhaseIteration(Atom previewesPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

            while (IsRunning)
            {
                new WaitForSeconds(UpdateTiming);
                stayontime += Time.deltaTime;

                // Go to Screensaverphase
                if (ScreenSaverPhase != null && stayontime > MaxStayOnTime)
                {
                    Controller.StartPhase(ScreenSaverPhase);
                }

                yield return null;
            }

            Debug.Log(String.Format("Ending Phase:{0}", gameObject.name.ToString()));
        }

        public void SelectByID(int id)
        {
            if (IsRunning)
            {
                lastSelection = selectionIndex;
                if (id >= 0 && id < SelectableItems.Length)
                {
                    selectionIndex = id;
                }

                doSelection();
            }

        }

        public void SelectLast()
        {
            if (IsRunning)
            {
                lastSelection = selectionIndex;
                if (selectionIndex == 0) { selectionIndex = SelectableItems.Length - 1; }
                else { selectionIndex--; }

                doSelection();
            }
            
        }

        public void SelectNext()
        {
            if (IsRunning)
            {
                lastSelection = selectionIndex;
                if (selectionIndex == SelectableItems.Length - 1) { selectionIndex = 0; }
                else { selectionIndex++; }

                doSelection();
            }
        }

        private void doSelection()
        {
            Debug.Log(selectionIndex);
            var s = SelectableItems[selectionIndex];
            if (s != null)
            {
                if (s.OnDeSelected != null)
                {
                    SelectableItems[lastSelection].OnDeSelected.Invoke();
                }

                if (s.OnSelected != null)
                {
                    SelectableItems[selectionIndex].OnSelected.Invoke();
                }
            }
        }

        public void Activate()
        {
            var s = SelectableItems[selectionIndex];
            if (s != null && IsRunning)
            {
                if (s.OnSelected != null)
                    SelectableItems[selectionIndex].OnSelected.Invoke();

                var NextPhase = SelectableItems[selectionIndex].Phase;
                Controller.StartPhase(NextPhase);
            }
        }
    }
}
