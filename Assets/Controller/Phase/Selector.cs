using UnityEngine;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Controller.Phase
{

    public class Selector : Atom
    {
        public Atom ScreenSaverPhase;
        public float MaxStayOnTime = 10f;
        public bool MouseInteraction = true;
        public LayerMask Layer;
        public bool KeyBoardInteraction = true;
        public KeyCode NextKey = KeyCode.UpArrow;
        public KeyCode PreviewsKey = KeyCode.DownArrow;
        public KeyCode AvtivateKey = KeyCode.Return;

        [SerializeField]
        public SelectorItem[] SelectableItems;

        private int selectionIndex = 0;
        public int SelectionIndex {
            get
            {
                return selectionIndex;
            }
        }
        private int lastSelection = 0;
        private float stayontime;

        public void Awake()
        {
            GetItems(gameObject.GetComponentsInChildren<SelectorActor>(false));
            doSelection();
        }

        public void GetItems (SelectorActor[] data)
        {
            foreach (var item in data)
            {
                AddActor(item);
            }
        }

        public void AddActor (SelectorActor data)
        {
            Phase item = data.GetComponent<Phase>();

            if (item != null && item.gameObject != gameObject)
            {
                SelectorItem si = new SelectorItem();
                si.Phase = item;
                item.Controller = base.Controller;

                SelectorActor sa = item.GetComponent<SelectorActor>();
                if (sa != null)
                {
                    si.OnActivated = new UnityEvent();
                    si.OnActivated.AddListener(new UnityAction(sa.Activate));

                    si.OnSelected = new UnityEvent();
                    si.OnSelected.AddListener(new UnityAction(sa.Select));

                    si.OnDeselected = new UnityEvent();
                    si.OnDeselected.AddListener(new UnityAction(sa.Deselect));
                }

                Collider co = item.GetComponent<Collider>();
                if (co != null)
                {
                    si.MouseCollider = co;
                }

                Array.Resize(ref SelectableItems, SelectableItems.Length + 1);
                SelectableItems[SelectableItems.Length - 1] = si;
            }
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

            if (Physics.Raycast(ray, out hit,Layer))
            {
                if (hit.collider != null)
                {
                    for (int i = 0; i < SelectableItems.Length; i++)
                    {
                        if (SelectableItems[i].MouseCollider != null && SelectableItems[i].MouseCollider.Equals(hit.collider))
                        {
                            //if (selectionIndex != i)
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
            if (Input.GetKeyDown(AvtivateKey))
                Activate();

            if (Input.GetKeyDown(NextKey))
                SelectNext();

            if (Input.GetKeyDown(PreviewsKey))
                SelectLast();
        }

        public void FixedUpdate()
        {
            if (IsRunning)
            {
                // MouseUpdate
                if (MouseInteraction)
                    MouseInput();

                // keyboardInput
                if (KeyBoardInteraction)
                    GetKeyBoardInput();
            }

        }

        public override IEnumerator PhaseIteration(Atom previewesPhase)
        {
            Debug.Log(String.Format("Start Phase:{0}", gameObject.name.ToString()));

            while (IsRunning)
            {
                Debug.Log(String.Format("Running Phase:{0}", gameObject.name.ToString()));

                new WaitForSeconds(Controller.UpdateTimming);
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
            if (IsRunning && SelectableItems.Length > 0)
            {
                Debug.Log(selectionIndex);
                var s = SelectableItems[selectionIndex];
                if (s != null)
                {
                    if (s.OnDeselected != null)
                    {
                        SelectableItems[lastSelection].OnDeselected.Invoke();
                    }

                    if (s.OnSelected != null)
                    {
                        SelectableItems[selectionIndex].OnSelected.Invoke();
                    }
                }
            }
            
        }

        public virtual void Activate()
        {
            var s = SelectableItems[selectionIndex];
            if (s != null && IsRunning)
            {
                if (s.OnSelected != null)
                    SelectableItems[selectionIndex].OnActivated.Invoke();

                var NextPhase = SelectableItems[selectionIndex].Phase;
                Controller.StartPhase(NextPhase);
            }
        }
    }
}
