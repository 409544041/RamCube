using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;
using Alias_rifegrt_Segments = E_Segments;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_SegmentsGameplayData")]
public partial class M_SegmentsGameplayData : BGEntityGo
{
	public override BGMetaEntity MetaConstraint
	{
		get
		{
			return MetaDefault;
		}
	}
	private static BansheeGz.BGDatabase.BGMetaRow _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaRow MetaDefault
	{
		get
		{
			if(_metaDefault==null || _metaDefault.IsDeleted) _metaDefault=BGRepo.I.GetMeta<BansheeGz.BGDatabase.BGMetaRow>(new BGId(4989130159753333157UL,7118120055729332661UL));
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
	public E_Segments f_Segment
	{
		get
		{
			return (E_Segments) _f_Segment[Entity.Index];
		}
		set
		{
			_f_Segment[Entity.Index] = value;
		}
	}
	public System.Boolean f_Rescued
	{
		get
		{
			return _f_Rescued[Entity.Index];
		}
		set
		{
			_f_Rescued[Entity.Index] = value;
		}
	}
	public System.Boolean f_AddedToSerpScreen
	{
		get
		{
			return _f_AddedToSerpScreen[Entity.Index];
		}
		set
		{
			_f_AddedToSerpScreen[Entity.Index] = value;
		}
	}
	public List<E_FluffDialogueGameplayData> f_FluffDialogueGameplayData
	{
		get
		{
			var val = _f_FluffDialogueGameplayData[Entity.Index];
			if(val==null || val.Count==0) return null;
			var ___FluffDialogueGameplayData = new List<E_FluffDialogueGameplayData>();
			for (var i = 0; i < val.Count; i++) ___FluffDialogueGameplayData.Add((E_FluffDialogueGameplayData) val[i]);
			return ___FluffDialogueGameplayData;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName __f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name
	{
		get
		{
			if(__f_name==null || __f_name.IsDeleted) __f_name=(BansheeGz.BGDatabase.BGFieldEntityName) MetaDefault.GetField(new BGId(5330384241163026195UL,12595475573590950033UL));
			return __f_name;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldRelationSingle __f_Segment;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_Segment
	{
		get
		{
			if(__f_Segment==null || __f_Segment.IsDeleted) __f_Segment=(BansheeGz.BGDatabase.BGFieldRelationSingle) MetaDefault.GetField(new BGId(5163421334888915869UL,10163773687724329907UL));
			return __f_Segment;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_Rescued;
	public static BansheeGz.BGDatabase.BGFieldBool _f_Rescued
	{
		get
		{
			if(__f_Rescued==null || __f_Rescued.IsDeleted) __f_Rescued=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(4897271965161504167UL,2768130027215449236UL));
			return __f_Rescued;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldBool __f_AddedToSerpScreen;
	public static BansheeGz.BGDatabase.BGFieldBool _f_AddedToSerpScreen
	{
		get
		{
			if(__f_AddedToSerpScreen==null || __f_AddedToSerpScreen.IsDeleted) __f_AddedToSerpScreen=(BansheeGz.BGDatabase.BGFieldBool) MetaDefault.GetField(new BGId(5472508438153550995UL,3271020857625076629UL));
			return __f_AddedToSerpScreen;
		}
	}
	private static BansheeGz.BGDatabase.BGFieldNested __f_FluffDialogueGameplayData;
	public static BansheeGz.BGDatabase.BGFieldNested _f_FluffDialogueGameplayData
	{
		get
		{
			if(__f_FluffDialogueGameplayData==null || __f_FluffDialogueGameplayData.IsDeleted) __f_FluffDialogueGameplayData=(BansheeGz.BGDatabase.BGFieldNested) MetaDefault.GetField(new BGId(5289789315245825160UL,11581812223873279893UL));
			return __f_FluffDialogueGameplayData;
		}
	}
	private static readonly List<BGEntity> _tugjbyuhfv_reusableList = new List<BGEntity>();
	public List<E_Segments> RelatedSegmentsListUsingGameplayDataRelation
	{
		get
		{
			var _private_related_list = Alias_rifegrt_Segments._f_GameplayData.GetRelatedIn(Entity.Id, _tugjbyuhfv_reusableList);
			if (_private_related_list.Count == 0) return null;
			var _private_result_list = new List<E_Segments>(_private_related_list.Count);
			for (var i = 0; i < _private_related_list.Count; i++) _private_result_list.Add((E_Segments) _private_related_list[i]);
			_tugjbyuhfv_reusableList.Clear();
			return _private_result_list;
		}
	}
}
