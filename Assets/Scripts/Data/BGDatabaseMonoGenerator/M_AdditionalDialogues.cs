using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_AdditionalDialogues")]
public partial class M_AdditionalDialogues : BGEntityGo
{
	public override BGMetaEntity MetaConstraint
	{
		get
		{
			return MetaDefault;
		}
	}
	private static BansheeGz.BGDatabase.BGMetaNested _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaNested MetaDefault
	{
		get
		{
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaNested>(new BGId(5425614183845625916UL,4516572263605434244UL));
			return _metaDefault;
		}
	}
	public static BansheeGz.BGDatabase.BGRepoEvents Events
	{
		get
		{
			return BGRepo.I.Events;
		}
	}
	public System.String f_name
	{
		get
		{
			return _f_name[Entity.Index];
		}
		set
		{
			_f_name[Entity.Index] = value;
		}
	}
	public E_Dialogues f_Dialogues
	{
		get
		{
			return (E_Dialogues) _f_Dialogues[Entity.Index];
		}
		set
		{
			_f_Dialogues[Entity.Index] = value;
		}
	}
	public System.Boolean f_Played
	{
		get
		{
			return _f_Played[Entity.Index];
		}
		set
		{
			_f_Played[Entity.Index] = value;
		}
	}
	public UnityEngine.ScriptableObject f_AdditionalDialogue
	{
		get
		{
			return _f_AdditionalDialogue[Entity.Index];
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(5030845089153911581UL,7056475815551447424UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldRelationSingle __f_Dialogues;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_Dialogues
	{
		get
		{
			if(__f_Dialogues==null || __f_Dialogues.IsDeleted) __f_Dialogues=(BansheeGz.BGDatabase.BGFieldRelationSingle) MetaDefault.GetField(new BGId(5113335154165853325UL,13117729812889025414UL));
			return __f_Dialogues;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_Played;
	public static BansheeGz.BGDatabase.BGFieldBool _f_Played
	{
		get
		{
			if(__f_Played==null || __f_Played.IsDeleted) __f_Played=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5702079073593323336UL,2868386591389818757UL));
			return __f_Played;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldUnityScriptableObject __f_AdditionalDialogue;
	public static BansheeGz.BGDatabase.BGFieldUnityScriptableObject _f_AdditionalDialogue
	{
		get
		{
			if(__f_AdditionalDialogue==null || __f_AdditionalDialogue.IsDeleted) __f_AdditionalDialogue=(BansheeGz.BGDatabase.BGFieldUnityScriptableObject) MetaDefault.GetField(new BGId(5187168267212154598UL,8395711673383198627UL));
			return __f_AdditionalDialogue;
		}
	}
}