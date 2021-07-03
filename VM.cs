using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//VM to interpret and process spells from their bytecodes
//NOTE: Need to split this into 2 scripts - a VM that can be accessed from anywhere and a game manager for use in spawning spells
public class VM : MonoBehaviour
{
    //VM paramters, set max size of the stack and the current index in the stack
    [SerializeField]
    int stackSize = 32;
    int stackIndex = -1;
    int[] stack = default;

    //Current state of the VM, used in state machine pattern
    public enum VMState { idle, targetting};
    public VMState vmState = VMState.idle;
    //Instruction types, defines possible actions used in spell processing
    enum Instruction { LiteralValue, NewSpell, SetElement, SetGameObject, SetPosition, SetRange, SetDuration};
    //Types of available spells
    enum SpellType { Construct, SelfTarget, AreaOfEffect };

    //List of game objects that can be spawned by spells
    [SerializeField]
    GameObject[] spawnableObjects = default;

    //Reference to spell list dropdown menu, populated by the list of spell bytecode file names
    [SerializeField]
    Dropdown spellsDropdown = default;
    List<string> spellNames = new List<string>();
    List<int[]> spellBytecodes = new List<int[]>();
    int spellIndex = 0;
    ISpellType currentSpell = null;

    //Reference to targetting component, used when casting certain spell types in order to control selecting the position and rotation of a spell using a mouse
    [SerializeField]
    TargettingPointer targetPointer;

    //Initialise the stack and list of available spells in the spell list dropdown menu
    private void Awake()
    {
        stack = new int[stackSize];
        spellBytecodes = FileIO.LoadSpellFiles(out spellNames);
        spellsDropdown.ClearOptions();
        spellsDropdown.AddOptions(spellNames);
    }

    //Push a new value onto the end of the stack, if the stack has reached the maximum size discard it and warn the user
    void Push(int value)
    {
        if (stackIndex < stack.Length)
        {
            stackIndex += 1;
            stack[stackIndex] = value;
        }
        else
        {
            Debug.Log("Max stack size reached, value discarded");
        }
    }

    //Pop the end value from the stack and return it, if there are no values in the stack return 0 and warn the user
    int Pop()
    {
        if(stackIndex >= 0)
        {
            int value = stack[stackIndex];
            stackIndex -= 1;
            return value;
        } else
        {
            Debug.Log("No values in the stack, returning zero");
            return 0;
        }
    }

    //Update loop runs the code appropriate for the current state of the VM
    private void Update()
    {
        switch (vmState)
        {
            //No spell is beign cast, wait for user input
            case VMState.idle:
                break;
            //User has requested a spell to be cast which involves placing an object in the world, stay in this state until the left mouse is clicked whilst there is a valid
            //position to cast the spell on, or if the user presses "Esc" end targetting
            case VMState.targetting:
                if (currentSpell.Target() && Input.GetMouseButtonDown(0))
                {
                    currentSpell.Cast();
                    targetPointer.DisableTargetting();
                    vmState = VMState.idle;
                }
                else if (Input.GetButtonDown("Cancel"))
                {
                    targetPointer.DisableTargetting();
                    vmState = VMState.idle;
                }
                break;
            default:
                break;
        }
    }

    public void SelectSpell(Player player)
    {
        //Select spell bytecode
        int[] byteCode = spellBytecodes[spellIndex];

        //Create spell object
        currentSpell = ProcessSpell(byteCode);
        
        //Start spell targetting
        currentSpell.StartTargetting(targetPointer);
        targetPointer.ActivateTargetting();
        vmState = VMState.targetting;
    }

    int[] LoadSpell(string spellName)
    {
        int[] byteCode = FileIO.GetByteCode(spellName);
        return byteCode;
    }

    ISpellType ProcessSpell(int[] bytecode)
    {
        ISpellType spell = default;

        for(int i = 0; i < bytecode.Length; i++)
        {
            Instruction instruction = (Instruction)bytecode[i];
            switch (instruction)
            {
                case Instruction.LiteralValue:
                    i += 1;
                    int value = bytecode[i];
                    Push(value);
                    break;

                case Instruction.NewSpell:
                    SpellType spellType = (SpellType)Pop();
                    switch (spellType)
                    {
                        case SpellType.Construct:
                            spell = new Construct();
                            targetPointer.SetTargetFloor(false);
                            break;
                        case SpellType.SelfTarget:
                            spell = new SelfTarget();
                            break;
                        case SpellType.AreaOfEffect:
                            spell = new AreaOfEffect();
                            targetPointer.SetTargetFloor(true);
                            break;
                        default:
                            spell = null;
                            break;
                    }
                    break;

                case Instruction.SetElement:
                    Element.ElementType element = (Element.ElementType)Pop();
                    spell.SetElement(element);
                    break;

                case Instruction.SetGameObject:
                    int objectIndex = Pop();
                    if(objectIndex >= spawnableObjects.Length)
                    {
                        objectIndex = spawnableObjects.Length - 1;
                    }
                    spell.SetGameObject(spawnableObjects[objectIndex]);
                    break;

                case Instruction.SetPosition:
                    int x = Pop();
                    int y = Pop();
                    int z = Pop();
                    spell.SetPosition(new Vector3(x, y, z));
                    break;

                case Instruction.SetRange:
                    int range = Pop();
                    spell.SetRange(range);
                    break;

                case Instruction.SetDuration:
                    int duration = Pop();
                    spell.SetDuration(duration);
                    break;
                default:
                    break;
            }
        }
        return spell;
    }

    public void SetSpellIndex(int index)
    {
        if(index < 0)
        {
            spellIndex = 0;
        } 
        else if(index >= spellNames.Count)
        {
            spellIndex = spellNames.Count - 1;
        }
        else
        {
            spellIndex = index;
        }
    }
}
