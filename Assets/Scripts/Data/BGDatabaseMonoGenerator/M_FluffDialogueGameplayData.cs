using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_FluffDialogueGameplayData")]
public partial class M_FluffDialogueGameplayData : BGEntityGo
{
	public override BGMetaEntity MetaConstraint => MetaDefault;
	private static BansheeGz.BGDatabase.BGMetaNested _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaNested MetaDefault => _metaDefault ?? (_metaDefault = BGCodeGenUtils.GetMeta<BansheeGz.BGDatabase.BGMetaNested>(new BGId(5635743854043088856UL,17727459658554333612UL), () => _metaDefault = null));
	public static BansheeGz.BGDatabase.BGRepoEvents Events => BGRepo.I.Events;
	public System.String f_name
	{
		get => _f_name[Entity.Index];
		set => _f_name[Entity.Index] = value;
	}
	public E_SegmentsGameplayData f_SegmentsGameplayData
	{
		get => (E_SegmentsGameplayData) _f_SegmentsGameplayData[Entity.Index];
		set => _f_SegmentsGameplayData[Entity.Index] = value;
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName _ufle12jhs77_f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name => _ufle12jhs77_f_name ?? (_ufle12jhs77_f_name = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldEntityName>(MetaDefault, new BGId(5084205847461478916UL, 6488143047621156767UL), () => _ufle12jhs77_f_name = null));
	private static BansheeGz.BGDatabase.BGFieldRelationSingle _ufle12jhs77_f_SegmentsGameplayData;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_SegmentsGameplayData => _ufle12jhs77_f_SegmentsGameplayData ?? (_ufle12jhs77_f_SegmentsGameplayData = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldRelationSingle>(MetaDefault, new BGId(5633271626414292479UL, 9592474805348906911UL), () => _ufle12jhs77_f_SegmentsGameplayData = null));
}
