using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace NagaisoraFamework
{
	using static MainSystem;

	[AddComponentMenu("Event/Standalone Input Module")]
	public class InputModule : PointerInputModule
	{
		public enum InputMode
		{
			Mouse,
			Buttons
		}

		public bool mouseAbled;

		public string horizontalAxis = "Horizontal";

		public string verticalAxis = "Vertical";

		public Vector2 AxisVector;

		public KeyConfig KeyConfig;

		protected StandaloneInputModule _instance;

		public float PrevActionTime;

		public Vector2 LastMoveVector;

		public int ConsecutiveMoveCount;

		public Vector2 LastMousePosition;

		public Vector2 MousePosition;

		public GameObject CurrentFocusedGameObject;

		public PointerEventData InputPointerEvent;

		public float inputActionsPerSecond = 10f;

		public float repeatDelay = 0.5f;

		public bool forceModuleActive;

		public static InputModule Instance { get; set; }

		public InputMode inputMode => InputMode.Mouse;

		public ushort DownKey;

		public bool AllowActivationOnMobileDevice
		{
			get
			{
				return forceModuleActive;
			}
			set
			{
				forceModuleActive = value;
			}
		}

		public bool ForceModuleActive
		{
			get
			{
				return forceModuleActive;
			}
			set
			{
				forceModuleActive = value;
			}
		}

		public float InputActionsPerSecond
		{
			get
			{
				return inputActionsPerSecond;
			}
			set
			{
				inputActionsPerSecond = value;
			}
		}

		public float RepeatDelay
		{
			get
			{
				return repeatDelay;
			}
			set
			{
				repeatDelay = value;
			}
		}

		public string HorizontalAxis
		{
			get
			{
				return horizontalAxis;
			}
			set
			{
				horizontalAxis = value;
			}
		}

		public string VerticalAxis
		{
			get
			{
				return verticalAxis;
			}
			set
			{
				verticalAxis = value;
			}
		}

		public int count = 0;

		protected override void Awake()
		{
			MainSystem.InputModule = this;

			Instance = this;
			base.Awake();
			Cursor.visible = mouseAbled;

			KeyConfig = KeyConfig.Default;
		}

		public void Update()
		{
			BitArray bitArray = new(16, false);

			AxisVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (AxisVector.x != 0 || AxisVector.y != 0)
			{
				if (AxisVector.y > 0.5f)
				{
					bitArray[0] = true;
				}
				if (AxisVector.y < -0.5f)
				{
					bitArray[1] = true;
				}
				if (AxisVector.x < -0.5f)
				{
					bitArray[2] = true;
				}
				if (AxisVector.x > 0.5f)
				{
					bitArray[3] = true;
				}
			}

			if (GetKeys(KeyConfig.SubmitKeys) || Input.GetKey(KeyConfig.J_SubmitKey))
			{
				bitArray[4] = true;
			}
			if (GetKeys(KeyConfig.CancelKeys) || Input.GetKey(KeyConfig.J_CancelKey))
			{
				bitArray[5] = true;
			}

			if (Input.GetKey(KeyConfig.Slow) || Input.GetKey(KeyConfig.J_Slow))
			{
				bitArray[6] = true;
			}

			byte[] data = new byte[bitArray.Count / 8];
			bitArray.CopyTo(data, 0);
			DownKey = BitConverter.ToUInt16(data);
		}

		public void FixedUpdate()
		{
			CallKeyDown(DownKey);
		}

		public override void Process()
		{
			if (!base.eventSystem.isFocused)
			{
				return;
			}
			bool flag = SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					SendSubmitEventToSelectedObject();
				}
			}
			if (mouseAbled)
			{
				if (!Cursor.visible)
				{
					Cursor.visible = true;
				}
				if (!ProcessTouchEvents() && base.input.mousePresent)
				{
					ProcessMouseEvent();
				}
			}
			else if (Cursor.visible)
			{
				Cursor.visible = false;
			}
		}

		protected InputModule()
		{
		}

		public static bool GetKeysDown(KeyCode[] keys)
		{
			bool flag = false;
			foreach (KeyCode submitKey in keys)
			{
				flag |= Input.GetKeyDown(submitKey);
			}
			return flag;
		}

		public static bool GetKeys(KeyCode[] keys)
		{
			bool flag = false;
			foreach (KeyCode submitKey in keys)
			{
				flag |= Input.GetKey(submitKey);
			}
			return flag;
		}

		public static bool GetKeysUp(KeyCode[] keys)
		{
			bool flag = false;
			foreach (KeyCode submitKey in keys)
			{
				flag |= Input.GetKeyUp(submitKey);
			}
			return flag;
		}

		public bool ShouldIgnoreEventsOnNoFocus()
		{
			OperatingSystemFamily operatingSystemFamily = SystemInfo.operatingSystemFamily;
			if ((uint)(operatingSystemFamily - 1) <= 2u)
			{
				return true;
			}
			return false;
		}

		public override void UpdateModule()
		{
			if (!base.eventSystem.isFocused && ShouldIgnoreEventsOnNoFocus())
			{
				if (InputPointerEvent != null && InputPointerEvent.pointerDrag != null && InputPointerEvent.dragging)
				{
					ExecuteEvents.Execute(InputPointerEvent.pointerDrag, InputPointerEvent, ExecuteEvents.endDragHandler);
				}
				InputPointerEvent = null;
			}
			else
			{
				LastMousePosition = MousePosition;
				MousePosition = base.input.mousePosition;
			}
		}

		public override bool IsModuleSupported()
		{
			if (!forceModuleActive && !base.input.mousePresent)
			{
				return base.input.touchSupported;
			}
			return true;
		}

		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}

			bool flag = forceModuleActive;
			flag |= GetKeysDown(KeyConfig.SubmitKeys);
			flag |= Input.GetKey(KeyConfig.J_SubmitKey);
			flag |= GetKeysDown(KeyConfig.CancelKeys);
			flag |= Input.GetKey(KeyConfig.J_CancelKey);
			flag |= !Mathf.Approximately(input.GetAxisRaw(horizontalAxis), 0f);
			flag |= !Mathf.Approximately(input.GetAxisRaw(verticalAxis), 0f);
			flag |= Input.GetKey(KeyConfig.Left);
			flag |= Input.GetKey(KeyConfig.Right);
			flag |= Input.GetKey(KeyConfig.Up);
			flag |= Input.GetKey(KeyConfig.Down);
			flag |= (MousePosition - LastMousePosition).sqrMagnitude > 0f;
			flag |= input.GetMouseButtonDown(0);
			if (input.touchCount > 0)
			{
				flag = true;
			}
			return flag;
		}

		public override void ActivateModule()
		{
			if (base.eventSystem.isFocused || !ShouldIgnoreEventsOnNoFocus())
			{
				base.ActivateModule();
				MousePosition = base.input.mousePosition;
				LastMousePosition = base.input.mousePosition;
				GameObject gameObject = base.eventSystem.currentSelectedGameObject;
				if (gameObject == null)
				{
					gameObject = base.eventSystem.firstSelectedGameObject;
				}
				base.eventSystem.SetSelectedGameObject(gameObject, GetBaseEventData());
			}
		}

		public override void DeactivateModule()
		{
			base.DeactivateModule();
			ClearSelection();
		}

		public bool ProcessTouchEvents()
		{
			for (int i = 0; i < base.input.touchCount; i++)
			{
				Touch touch = base.input.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					PointerEventData touchPointerEventData = GetTouchPointerEventData(touch, out bool pressed, out bool released);
					ProcessTouchPress(touchPointerEventData, pressed, released);
					if (!released)
					{
						ProcessMove(touchPointerEventData);
						ProcessDrag(touchPointerEventData);
					}
					else
					{
						RemovePointerData(touchPointerEventData);
					}
				}
			}
			return base.input.touchCount > 0;
		}

		protected void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					if (unscaledTime - pointerEvent.clickTime < 0.3f)
					{
						pointerEvent.clickCount++;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
				InputPointerEvent = pointerEvent;
			}
			if (released)
			{
				ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
				InputPointerEvent = pointerEvent;
			}
		}

		protected bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			if (GetKeysDown(KeyConfig.SubmitKeys) || Input.GetKeyDown(KeyConfig.J_SubmitKey))
			{
				ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
			}
			if (GetKeysDown(KeyConfig.CancelKeys) || Input.GetKeyDown(KeyConfig.J_CancelKey))
			{
				ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
			}
			return baseEventData.used;
		}

		public Vector2 GetRawMoveVector()
		{
			Vector2 zero = Vector2.zero;
			zero.x = base.input.GetAxisRaw(horizontalAxis);
			zero.y = base.input.GetAxisRaw(verticalAxis);
			if (base.input.GetButtonDown(horizontalAxis))
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (base.input.GetButtonDown(verticalAxis))
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			if (Input.GetKey(KeyConfig.Left))
			{
				zero.x += -1f;
			}
			if (Input.GetKey(KeyConfig.Right))
			{
				zero.x += 1f;
			}
			if (Input.GetKey(KeyConfig.Down))
			{
				zero.y += -1f;
			}
			if (Input.GetKey(KeyConfig.Up))
			{
				zero.y += 1f;
			}
			return zero;
		}

		protected bool SendMoveEventToSelectedObject()
		{
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = false;
			bool flag2 = Vector2.Dot(rawMoveVector, LastMoveVector) > 0f;
			if (!flag)
			{
				flag = ((!flag2 || ConsecutiveMoveCount != 1) ? (unscaledTime > PrevActionTime + 1f / inputActionsPerSecond) : (unscaledTime > PrevActionTime + repeatDelay));
			}
			if (!flag)
			{
				return false;
			}
			AxisEventData axisEventData = GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				if (!flag2)
				{
					ConsecutiveMoveCount = 0;
				}
				ConsecutiveMoveCount++;
				PrevActionTime = unscaledTime;
				LastMoveVector = rawMoveVector;
			}
			else
			{
				ConsecutiveMoveCount = 0;
			}
			return axisEventData.used;
		}

		protected void ProcessMouseEvent()
		{
			ProcessMouseEvent(0);
		}

		protected virtual bool ForceAutoSelect()
		{
			return false;
		}

		protected void ProcessMouseEvent(int id)
		{
			MouseState mousePointerEventData = GetMousePointerEventData(id);
			MouseButtonEventData eventData = mousePointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;
			CurrentFocusedGameObject = eventData.buttonData.pointerCurrentRaycast.gameObject;
			ProcessMousePress(eventData);
			ProcessMove(eventData.buttonData);
			ProcessDrag(eventData.buttonData);
			ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
			ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);
			ProcessMousePress(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
			ProcessDrag(mousePointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);
			if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
			{
				ExecuteEvents.ExecuteHierarchy(ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject), eventData.buttonData, ExecuteEvents.scrollHandler);
			}
		}

		protected bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		protected void ProcessMousePress(MouseButtonEventData data)
		{
			PointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					if (unscaledTime - buttonData.clickTime < 0.3f)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
				InputPointerEvent = buttonData;
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					HandlePointerExitAndEnter(buttonData, null);
					HandlePointerExitAndEnter(buttonData, gameObject);
				}
				InputPointerEvent = buttonData;
			}
		}

		protected GameObject GetCurrentFocusedGameObject()
		{
			return CurrentFocusedGameObject;
		}
	}
}