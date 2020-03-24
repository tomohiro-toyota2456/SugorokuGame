using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// AIの編集ウィンドウ
/// </summary>
public class AIEditorWindow : EditorWindow
{
	AIEditorGraphView graphView;

	string openingFilePath = "";

	[MenuItem("AI/Editor/OpenWindow")]
	public static void Open()
	{
		GetWindow<AIEditorWindow>();
	}

	public void OnEnable()
	{
		//ツールバーセット
		rootVisualElement.Add(SetupToolbar());

		//グラフビューセット
		InitGraphView();
	}

	void InitGraphView()
	{
		if(graphView != null)
		{
			rootVisualElement.Remove(graphView);
		}

		graphView = new AIEditorGraphView()
		{
			style = { flexGrow = 1 }
		};
		rootVisualElement.Add(graphView);
	}

	Toolbar SetupToolbar()
	{
		Toolbar toolbar = new Toolbar();
		ToolbarMenu menu = new ToolbarMenu();
		menu.text = "File";
		menu.menu.AppendAction("New File", CreateNewFile);
		menu.menu.AppendAction("Open File", OpenFile);
		menu.menu.AppendSeparator();
		menu.menu.AppendAction("Save File", SaveFile);
		menu.menu.AppendAction("Overwrite File", SaveOverwriteFile);

		toolbar.Add(menu);


		menu = new ToolbarMenu();
		menu.text = "Export";
		menu.menu.AppendAction("Export AIThinkingData", ExportAIFile);

		toolbar.Add(menu);

		return toolbar;
	}

	#region MenuActions
	void CreateNewFile(DropdownMenuAction menuAct)
	{
		if(string.IsNullOrEmpty(openingFilePath))
		{
			openingFilePath = "";
			InitGraphView();
			return;
		}

		
		if (EditorUtility.DisplayDialog("警告", "現在編集中のデータがありますが、保存せず新しいファイルを作りますか？", "はい", "いいえ"))
		{
			openingFilePath = "";
			InitGraphView();
			return;
		}

	}

	void SaveFile(DropdownMenuAction menuAct)
	{
		var path = EditorUtility.SaveFilePanelInProject("Save File", "New AIEditorFile", "asset","");

		if(string.IsNullOrEmpty(path))
		{
			return;
		}

		SaveData(path);
	}

	void SaveOverwriteFile(DropdownMenuAction menuAct)
	{
		if(string.IsNullOrEmpty(openingFilePath))
		{
			SaveFile(menuAct);
			return;
		}
	}

	void SaveData(string path)
	{
		var rootNode = graphView.GetRootNode();

		if(rootNode == null)
		{
			var ins = ScriptableObject.CreateInstance<AIEditorSaveFile>();
			AssetDatabase.CreateAsset(ins, path);
			return;
		}

		List<IAIAction> actions = new List<IAIAction>();
		List<Rect> positions = new List<Rect>();

		//ルートノードの保存用データ作成
		var cAIObj = rootNode as AIEditorBaseNode;
		Rect rect = cAIObj.GetPosition();
		object obj = cAIObj.GetData();
		Type type = obj.GetType();
		AIActionBase act = Cast(type.Name, obj);
		act.parent = null;
		actions.Add(act as IAIAction);
		positions.Add(rect);
		//再帰でつながりを作成
		CreateNodeRecursion(cAIObj, act, actions, positions);

		var instance = CreateInstance<AIEditorSaveFile>();
		instance.actions = actions;
		instance.rects = positions;

		AssetDatabase.CreateAsset(instance, path);
	}

	void OpenFile(DropdownMenuAction menuAct)
	{
		var path = EditorUtility.OpenFilePanel("OpenFile", Application.dataPath, "asset");

		if (string.IsNullOrEmpty(path))
		{
			return;
		}

		int idx = path.IndexOf("Assets");

		openingFilePath = path;

		OpenData(path.Substring(idx));
		
	}

	void OpenData(string path)
	{
		var asset = AssetDatabase.LoadAssetAtPath<AIEditorSaveFile>(path);

		InitGraphView();
		CreateNodeRecursionForGraphView(asset.actions[0],null,asset);
	}

	void CreateNodeRecursionForGraphView(IAIAction action,AIEditorBaseNode parent,AIEditorSaveFile saveFile)
	{
		var node = CreateNode(action,saveFile);

		if(parent != null)
		{
			 graphView.AddElement(parent.ConnectTo(node.InputPort));
		}

		var actionBase = action as AIActionBase;

		if (actionBase.children == null)
			return;

		foreach(var child in actionBase.children)
		{
			CreateNodeRecursionForGraphView(child, node,saveFile);
		}

	}

	AIEditorBaseNode CreateNode(IAIAction action,AIEditorSaveFile saveFile)
	{
		Type type = action.GetType();
		AIEditorBaseNode node;
		switch(type.Name)
		{
			case "AIDiceAction":
				node = new UsingDiceActionNode();
				break;
			case "AISelector":
				node = new SelectorNode();
				break;
			case "AISequence":
				node = new SequenceNode();
				break;

			default:
				Debug.LogError("Err");
				return null;
		}

		node.SetData(action);

		Rect r = saveFile.SearchRect(action);
		node.SetPosition(r);
		graphView.AddElement(node);
		return node;
	}

	void ExportAIFile(DropdownMenuAction menuAct)
	{
		var path = EditorUtility.SaveFilePanelInProject("Save File", "New AIFile", "asset", "");

		if (string.IsNullOrEmpty(path))
		{
			return;
		}

		ExportAIData(path);
		
	}

	void ExportAIData(string path)
	{
		var rootNode = graphView.GetRootNode();

		if (rootNode == null)
		{
			return;
		}

		List<IAIAction> actions = new List<IAIAction>();
		List<Rect> positions = new List<Rect>();

		var cAIObj = rootNode as AIEditorBaseNode;
		Rect rect = cAIObj.GetPosition();
		object obj = cAIObj.GetData();
		Type type = obj.GetType();
		AIActionBase act = Cast(type.Name, obj);
		act.parent = null;
		actions.Add(act as IAIAction);
		positions.Add(rect);
		CreateNodeRecursion(cAIObj, act, actions, positions);

		var instance = CreateInstance<AIThinkingData>();
		Type insType = instance.GetType();

		var field = insType.GetField("actions", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

		field.SetValue(instance, actions);

		AssetDatabase.CreateAsset(instance, path);
	}

	void CreateNodeRecursion(AIEditorBaseNode node,AIActionBase actNode,List<IAIAction> actions,List<Rect> positions)
	{
		foreach(var child in node.GetChildren())
		{
			var cAIObj = child as AIEditorBaseNode;
			Rect rect = child.GetPosition();
			object obj = cAIObj.GetData();
			Type type = obj.GetType();
			AIActionBase act = Cast(type.Name, obj);


			act.parent = actNode as IAIAction;
			actions.Add(act as IAIAction);
			positions.Add(rect);

			if(actNode.children == null)
			{
				actNode.children = new List<IAIAction>();
			}

			actNode.children.Add(act as IAIAction);

			CreateNodeRecursion(cAIObj, act, actions, positions);
		}
	}

	AIActionBase Cast(string typeName,object obj)
	{
		switch(typeName)
		{
			case "AIDiceAction":
				return (AIDiceAction)obj;
			case "AISelector":
				return (AISelector)obj;
			case "AISequence":
				return (AISequence)obj;
		}
		return null;
	}
	#endregion

}

/// <summary>
/// グラフビュー
/// </summary>
public class AIEditorGraphView : GraphView
{
	AIEditorNodeSearchWindowProvider provider;
	public AIEditorGraphView() : base()
	{
		SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
		this.AddManipulator(new SelectionDragger());
		provider = ScriptableObject.CreateInstance<AIEditorNodeSearchWindowProvider>();
		provider.Initialize(this);
		nodeCreationRequest += RequestCreationNode;
	}

	/// <summary>
	/// ノード生成リクエスト
	/// </summary>
	/// <param name="context"></param>
	void RequestCreationNode(NodeCreationContext context)
	{
		SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), provider);
	}

	/// <summary>
	/// リンクルール
	/// </summary>
	/// <param name="startPort"></param>
	/// <param name="nodeAdapter"></param>
	/// <returns></returns>
	public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
	{
		var compatiblePorts = new List<Port>();

		var s = ports.ToList();

		foreach (var port in ports.ToList())
		{
			if(startPort.node == port.node || startPort.direction == port.direction || startPort.portType != port.portType)
			{
				continue;
			}

			var inputPort = startPort.direction == Direction.Input ? startPort : port;
			var outputPort = startPort.direction == Direction.Input ? port : startPort;

			if(CheckConnection(outputPort,inputPort))
			{
				continue;
			}

			compatiblePorts.Add(port);
		}
		return compatiblePorts;
	}

	bool CheckConnection(Port outputPort, Port inputPort)
	{
		var outNode = outputPort.node;

		AIEditorBaseNode inNode = inputPort.node as AIEditorBaseNode;

		return inNode.CheckConnectionOutputPort(outNode);
	}

	public List<Node> GetAllNode()
	{
		return nodes.ToList();
	}

	public AIEditorBaseNode GetRootNode()
	{
		foreach(var node in nodes.ToList())
		{
			var baseNode = node as AIEditorBaseNode;

			if(!baseNode.IsHavingParentNode())
			{
				return baseNode;
			}
		}

		return null;
	}

}

/// <summary>
/// 生成ノードルール
/// </summary>
public class AIEditorNodeSearchWindowProvider : ScriptableObject , ISearchWindowProvider
{
	private AIEditorGraphView graphView;

	public void Initialize(AIEditorGraphView gv)
	{
		graphView = gv;
	}

	List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
	{
		var entries = new List<SearchTreeEntry>();
		entries.Add(new SearchTreeGroupEntry(new GUIContent("Create Node")));

		foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach(var type in assembly.GetTypes())
			{
				if(type.IsClass && !type.IsAbstract && (type.IsSubclassOf(typeof(AIEditorBaseNode))))
				{
					entries.Add(new SearchTreeEntry(new GUIContent(type.Name)) { level = 1, userData = type });
				}
			}
		}

		return entries;
	}

	bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
	{
		var type = searchTreeEntry.userData as System.Type;
		var node = Activator.CreateInstance(type) as AIEditorBaseNode;
		graphView.AddElement(node);
		return true;
	}
}

/// <summary>
/// ノードベース
/// </summary>
public abstract class AIEditorBaseNode : Node
{
	protected Port inputPort;
	protected Port outputPort;

	public AIEditorBaseNode(bool isUsingOutputPort = true)
	{
		inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(Node));
		inputPort.portName = "In";
		inputContainer.Add(inputPort);

		if (isUsingOutputPort)
		{
			outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Node));
			outputPort.portName = "Out";
			outputContainer.Add(outputPort);
		}
	}

	public Edge ConnectTo(Port inputPort)
	{
		if (inputPort == null)
			return null;

		if (inputPort.direction != Direction.Input)
			return null;

		return outputPort.ConnectTo(inputPort);
	}

	public Port InputPort { get { return inputPort; } }

	public bool CheckConnectionOutputPort(Node node)
	{
		if (outputPort == null)
			return false;

		foreach(var edge in outputPort.connections)
		{
			if(node == edge.input.node)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckConnectionInputPort(Node node)
	{
		foreach (var edge in inputPort.connections)
		{
			if (node == edge.output.node)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsHavingParentNode()
	{
		return inputPort.connected;
	}

	public List<Node> GetChildren()
	{
		List<Node> list = new List<Node>();

		if(outputPort == null)
		{
			return list;
		}

		foreach( var edge in outputPort.connections)
		{
			list.Add(edge.input.node);
		}
		return list;
	}

	public abstract object GetData();
	public abstract void SetData(object obj);
}

public class SequenceNode : AIEditorBaseNode
{
	AISequence sequence = new AISequence();

	public SequenceNode() : base()
	{
		title = "AND";
	}

	public override object GetData()
	{
		return sequence;
	}

	public override void SetData(object obj)
	{
		sequence = obj as AISequence;
	}
}

public class SelectorNode : AIEditorBaseNode
{
	AISelector selector = new AISelector();

	public SelectorNode() : base()
	{
		title = "OR";
	}

	public override object GetData()
	{
		return selector;
	}

	public override void SetData(object obj)
	{
		selector = obj as AISelector;
	}
}

public class UsingDiceActionNode : AIEditorBaseNode
{
	AIDiceAction diceAction = new AIDiceAction();

	public UsingDiceActionNode() : base(false)
	{
		title = "USE DICE";
	}

	public override object GetData()
	{
		return diceAction;
	}

	public override void SetData(object obj)
	{
		diceAction = obj as AIDiceAction;
	}
}
