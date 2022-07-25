using System;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGDatabase;

//=============================================================
//||                   Generated by BansheeGz Code Generator ||
//=============================================================

[AddComponentMenu("BansheeGz/Generated/M_InGameDialogue")]
public partial class M_InGameDialogue : BGEntityGo
{
	public override BGMetaEntity MetaConstraint => MetaDefault;
	private static BansheeGz.BGDatabase.BGMetaNested _metaDefault;
	public static BansheeGz.BGDatabase.BGMetaNested MetaDefault => _metaDefault ?? (_metaDefault = BGCodeGenUtils.GetMeta<BansheeGz.BGDatabase.BGMetaNested>(new BGId(5643745451162196947UL,13920282399988847515UL), () => _metaDefault = null));
	public static BansheeGz.BGDatabase.BGRepoEvents Events => BGRepo.I.Events;
	public System.String f_name
	{
		get => _f_name[Entity.Index];
		set => _f_name[Entity.Index] = value;
	}
	public E_InGameDialogues f_InGameDialogues
	{
		get => (E_InGameDialogues) _f_InGameDialogues[Entity.Index];
		set => _f_InGameDialogues[Entity.Index] = value;
	}
	public Expressions f_Expression
	{
		get => (Expressions) _f_Expression[Entity.Index];
		set => _f_Expression[Entity.Index] = value;
	}
	public System.String f_LocalizedText
	{
		get => _f_LocalizedText[Entity.Index];
		set => _f_LocalizedText[Entity.Index] = value;
	}
	private static BansheeGz.BGDatabase.BGFieldEntityName _ufle12jhs77_f_name;
	public static BansheeGz.BGDatabase.BGFieldEntityName _f_name => _ufle12jhs77_f_name ?? (_ufle12jhs77_f_name = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldEntityName>(MetaDefault, new BGId(4894426744659196790UL, 18013493551907285656UL), () => _ufle12jhs77_f_name = null));
	private static BansheeGz.BGDatabase.BGFieldRelationSingle _ufle12jhs77_f_InGameDialogues;
	public static BansheeGz.BGDatabase.BGFieldRelationSingle _f_InGameDialogues => _ufle12jhs77_f_InGameDialogues ?? (_ufle12jhs77_f_InGameDialogues = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldRelationSingle>(MetaDefault, new BGId(5057580692058144049UL, 14186343624407140277UL), () => _ufle12jhs77_f_InGameDialogues = null));
	private static BansheeGz.BGDatabase.BGFieldEnum _ufle12jhs77_f_Expression;
	public static BansheeGz.BGDatabase.BGFieldEnum _f_Expression => _ufle12jhs77_f_Expression ?? (_ufle12jhs77_f_Expression = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldEnum>(MetaDefault, new BGId(5346416093139221045UL, 14852607355323933348UL), () => _ufle12jhs77_f_Expression = null));
	private static BansheeGz.BGDatabase.BGFieldLocalizedText _ufle12jhs77_f_LocalizedText;
	public static BansheeGz.BGDatabase.BGFieldLocalizedText _f_LocalizedText => _ufle12jhs77_f_LocalizedText ?? (_ufle12jhs77_f_LocalizedText = BGCodeGenUtils.GetField<BansheeGz.BGDatabase.BGFieldLocalizedText>(MetaDefault, new BGId(5112307064703955890UL, 2192105288789931158UL), () => _ufle12jhs77_f_LocalizedText = null));
}
