//Maya ASCII 2013 scene
//Name: Attack_Cycle.ma
//Last modified: Wed, Jul 16, 2014 10:39:55 AM
//Codeset: 1252
requires maya "2013";
requires "stereoCamera" "10.0";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2013";
fileInfo "version" "2013 x64";
fileInfo "cutIdentifier" "201209210409-845513";
fileInfo "osv" "Microsoft Windows 7 Business Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
fileInfo "license" "education";
createNode clipLibrary -n "clipLibrary1";
	setAttr -s 18 ".cel[0].cev";
	setAttr ".cd[0].cm" -type "characterMapping" 18 "Hand_R.rotateZ" 2 1 "Hand_R.rotateY" 
		2 2 "Hand_R.rotateX" 2 3 "Hand_R.translateZ" 1 1 "Hand_R.translateY" 
		1 2 "Hand_R.translateX" 1 3 "Torso.rotateZ" 2 4 "Torso.rotateY" 
		2 5 "Torso.rotateX" 2 6 "Torso.translateZ" 1 4 "Torso.translateY" 
		1 5 "Torso.translateX" 1 6 "Hand_L.rotateZ" 2 7 "Hand_L.rotateY" 
		2 8 "Hand_L.rotateX" 2 9 "Hand_L.translateZ" 1 7 "Hand_L.translateY" 
		1 8 "Hand_L.translateX" 1 9  ;
	setAttr ".cd[0].cim" -type "Int32Array" 18 0 1 2 3 4
		 5 6 7 8 9 10 11 12 13 14 15 16
		 17 ;
createNode animClip -n "Attack_CycleSource";
	setAttr ".ihi" 0;
	setAttr -s 18 ".ac[0:17]" yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes;
	setAttr ".ss" 1;
	setAttr ".se" 32;
	setAttr ".ci" no;
createNode animCurveTA -n "Hand_R_rotateZ";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 7 ".ktv[0:6]"  1 0 6 50.242714805740746 10 50.238119983929849
		 14 78.306251795724592 21 78.306251795724592 26 26.617096588015553 32 0;
	setAttr -s 7 ".kit[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kot[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kix[0:6]"  1 1 1 1 1 0.31795424222946167 1;
	setAttr -s 7 ".kiy[0:6]"  0 0 0 0 0 -0.94810611009597778 0;
	setAttr -s 7 ".kox[0:6]"  1 1 1 1 1 0.31795424222946167 1;
	setAttr -s 7 ".koy[0:6]"  0 0 0 0 0 -0.94810611009597778 0;
createNode animCurveTA -n "Hand_R_rotateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 7 ".ktv[0:6]"  1 -0.00019626102304547176 6 -7.0781199461137616
		 10 -4.5866931417742585 14 -0.00019626102304547176 21 -0.00019626102304547176 26 -42.938223718462034
		 32 -0.00019626102304547176;
	setAttr -s 7 ".kit[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kot[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kix[0:6]"  0.93717265129089355 1 0.93767887353897095 
		1 1 1 0.93717265129089355;
	setAttr -s 7 ".kiy[0:6]"  -0.34886592626571655 0 0.34750306606292725 
		0 0 0 -0.34886592626571655;
	setAttr -s 7 ".kox[0:6]"  0.93717265129089355 1 0.93767887353897095 
		1 1 1 0.93717265129089355;
	setAttr -s 7 ".koy[0:6]"  -0.34886592626571655 0 0.34750306606292725 
		0 0 0 -0.34886592626571655;
createNode animCurveTA -n "Hand_R_rotateX";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 7 ".ktv[0:6]"  1 -5.4308625935686763 6 -110.13003800526238
		 10 -104.56450568134683 14 -69.099319775623599 21 -37.681004316546357 26 -23.748746983409706
		 32 -5.4308625935686763;
	setAttr -s 7 ".kit[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kot[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kix[0:6]"  1 1 0.49646744132041931 1 1 0.63142251968383789 
		1;
	setAttr -s 7 ".kiy[0:6]"  0 0 0.86805528402328491 0 0 0.77543890476226807 
		0;
	setAttr -s 7 ".kox[0:6]"  1 1 0.49646744132041931 1 1 0.63142257928848267 
		1;
	setAttr -s 7 ".koy[0:6]"  0 0 0.86805528402328491 0 0 0.77543896436691284 
		0;
createNode animCurveTL -n "Hand_R_translateZ";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 7 ".ktv[0:6]"  1 -0.70686967538550771 6 64.845530353139296
		 10 74.091304063949266 14 85.251177909734764 21 76.988151332375452 26 63.852934508865587
		 32 -0.70686967538550771;
	setAttr -s 7 ".kit[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kot[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kix[0:6]"  0.15853768587112427 0.029071666300296783 
		0.016333168372511864 1 1 0.005899032112210989 0.15853768587112427;
	setAttr -s 7 ".kiy[0:6]"  -0.98735290765762329 0.99957740306854248 
		0.99986660480499268 0 0 -0.99998259544372559 -0.98735290765762329;
	setAttr -s 7 ".kox[0:6]"  0.15853767096996307 0.029071664437651634 
		0.016333168372511864 1 1 0.005899032112210989 0.15853767096996307;
	setAttr -s 7 ".koy[0:6]"  -0.98735290765762329 0.99957728385925293 
		0.99986660480499268 0 0 -0.99998265504837036 -0.98735290765762329;
createNode animCurveTL -n "Hand_R_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 7 ".ktv[0:6]"  1 -0.42189480902176557 6 47.924793602731505
		 10 44.229476061462577 14 37.426722576232898 21 26.825138143793843 26 -1.1712842503014029
		 32 -0.42189480902176557;
	setAttr -s 7 ".kit[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kot[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kix[0:6]"  0.8921770453453064 1 0.031735870987176895 
		1 1 1 0.8921770453453064;
	setAttr -s 7 ".kiy[0:6]"  -0.45168581604957581 0 -0.99949628114700317 
		0 0 0 -0.45168581604957581;
	setAttr -s 7 ".kox[0:6]"  0.8921770453453064 1 0.031735870987176895 
		1 1 1 0.8921770453453064;
	setAttr -s 7 ".koy[0:6]"  -0.45168581604957581 0 -0.99949628114700317 
		0 0 0 -0.45168581604957581;
createNode animCurveTL -n "Hand_R_translateX";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 7 ".ktv[0:6]"  1 -1.652802507151991 6 -12.031728436403752
		 10 -1.2984543403226247 14 18.460556069366955 21 69.681955526701898 26 14.939115570977309
		 32 -1.652802507151991;
	setAttr -s 7 ".kit[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kot[2:6]"  18 1 1 18 1;
	setAttr -s 7 ".kix[0:6]"  0.1076839491724968 1 0.010931073687970638 
		1 1 0.0064249732531607151 0.1076839491724968;
	setAttr -s 7 ".kiy[0:6]"  0.99418520927429199 0 0.99994027614593506 
		0 0 -0.99997931718826294 0.99418520927429199;
	setAttr -s 7 ".kox[0:6]"  0.1076839491724968 1 0.010931073687970638 
		1 1 0.0064249732531607151 0.1076839491724968;
	setAttr -s 7 ".koy[0:6]"  0.99418520927429199 0 0.99994027614593506 
		0 0 -0.99997943639755249 0.99418520927429199;
createNode animCurveTA -n "Torso_rotateZ";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 15 0 32 0;
	setAttr -s 3 ".kix[0:2]"  1 1 1;
	setAttr -s 3 ".kiy[0:2]"  0 0 0;
	setAttr -s 3 ".kox[0:2]"  1 1 1;
	setAttr -s 3 ".koy[0:2]"  0 0 0;
createNode animCurveTA -n "Torso_rotateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 15 15.184209505797265 32 0;
	setAttr -s 3 ".kix[0:2]"  1 1 1;
	setAttr -s 3 ".kiy[0:2]"  0 0 0;
	setAttr -s 3 ".kox[0:2]"  1 1 1;
	setAttr -s 3 ".koy[0:2]"  0 0 0;
createNode animCurveTA -n "Torso_rotateX";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 15 0 32 0;
	setAttr -s 3 ".kix[0:2]"  1 1 1;
	setAttr -s 3 ".kiy[0:2]"  0 0 0;
	setAttr -s 3 ".kox[0:2]"  1 1 1;
	setAttr -s 3 ".koy[0:2]"  0 0 0;
createNode animCurveTL -n "Torso_translateZ";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 15 0 32 0;
	setAttr -s 3 ".kix[0:2]"  1 1 1;
	setAttr -s 3 ".kiy[0:2]"  0 0 0;
	setAttr -s 3 ".kox[0:2]"  1 1 1;
	setAttr -s 3 ".koy[0:2]"  0 0 0;
createNode animCurveTL -n "Torso_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 15 0 32 0;
	setAttr -s 3 ".kix[0:2]"  1 1 1;
	setAttr -s 3 ".kiy[0:2]"  0 0 0;
	setAttr -s 3 ".kox[0:2]"  1 1 1;
	setAttr -s 3 ".koy[0:2]"  0 0 0;
createNode animCurveTL -n "Torso_translateX";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 15 0 32 0;
	setAttr -s 3 ".kix[0:2]"  1 1 1;
	setAttr -s 3 ".kiy[0:2]"  0 0 0;
	setAttr -s 3 ".kox[0:2]"  1 1 1;
	setAttr -s 3 ".koy[0:2]"  0 0 0;
createNode animCurveTA -n "Hand_L_rotateZ";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 0 16 0 21 0 32 0;
	setAttr -s 4 ".kot[2:3]"  18 1;
	setAttr -s 4 ".kix[0:3]"  1 1 1 1;
	setAttr -s 4 ".kiy[0:3]"  0 0 0 0;
	setAttr -s 4 ".kox[0:3]"  1 1 1 1;
	setAttr -s 4 ".koy[0:3]"  0 0 0 0;
createNode animCurveTA -n "Hand_L_rotateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 -0.00019626102304547176 16 13.537639223491157
		 21 34.891780057600876 32 -0.00019626102304547176;
	setAttr -s 4 ".kot[2:3]"  18 1;
	setAttr -s 4 ".kix[0:3]"  0.93717265129089355 1 1 0.93717265129089355;
	setAttr -s 4 ".kiy[0:3]"  -0.34886592626571655 0 0 -0.34886592626571655;
	setAttr -s 4 ".kox[0:3]"  0.93717265129089355 1 1 0.93717265129089355;
	setAttr -s 4 ".koy[0:3]"  -0.34886592626571655 0 0 -0.34886592626571655;
createNode animCurveTA -n "Hand_L_rotateX";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 0 16 31.561008912230893 21 31.561008912230893
		 32 0;
	setAttr -s 4 ".kot[2:3]"  18 1;
	setAttr -s 4 ".kix[0:3]"  1 1 1 1;
	setAttr -s 4 ".kiy[0:3]"  0 0 0 0;
	setAttr -s 4 ".kox[0:3]"  1 1 1 1;
	setAttr -s 4 ".koy[0:3]"  0 0 0 0;
createNode animCurveTL -n "Hand_L_translateZ";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 -3.3971065469923172 16 -0.65994016158992574
		 21 -7.3747406150746144 32 -3.3971065469923172;
	setAttr -s 4 ".kot[2:3]"  18 1;
	setAttr -s 4 ".kix[0:3]"  0.18346507847309113 1 1 0.18346507847309113;
	setAttr -s 4 ".kiy[0:3]"  0.98302626609802246 0 0 0.98302626609802246;
	setAttr -s 4 ".kox[0:3]"  0.18346504867076874 1 1 0.18346504867076874;
	setAttr -s 4 ".koy[0:3]"  0.98302614688873291 0 0 0.98302614688873291;
createNode animCurveTL -n "Hand_L_translateY";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 0 16 4.4560330368192318 21 11.127068183485058
		 32 0;
	setAttr -s 4 ".kot[2:3]"  18 1;
	setAttr -s 4 ".kix[0:3]"  1 1 1 1;
	setAttr -s 4 ".kiy[0:3]"  0 0 0 0;
	setAttr -s 4 ".kox[0:3]"  1 1 1 1;
	setAttr -s 4 ".koy[0:3]"  0 0 0 0;
createNode animCurveTL -n "Hand_L_translateX";
	setAttr ".tan" 1;
	setAttr ".wgt" no;
	setAttr -s 4 ".ktv[0:3]"  1 -4.7478070298084294 16 -12.058701169443934
		 21 -21.415684888813278 32 -4.7478070298084294;
	setAttr -s 4 ".kot[2:3]"  18 1;
	setAttr -s 4 ".kix[0:3]"  0.10259880870580673 1 1 0.10259880870580673;
	setAttr -s 4 ".kiy[0:3]"  0.99472278356552124 0 0 0.99472278356552124;
	setAttr -s 4 ".kox[0:3]"  0.10259881615638733 1 1 0.10259881615638733;
	setAttr -s 4 ".koy[0:3]"  0.99472278356552124 0 0 0.99472278356552124;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :renderPartition;
	setAttr -s 4 ".st";
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultShaderList1;
	setAttr -s 4 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :renderGlobalsList1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :defaultHardwareRenderGlobals;
	setAttr ".fn" -type "string" "im";
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
select -ne :characterPartition;
connectAttr "Attack_CycleSource.cl" "clipLibrary1.sc[0]";
connectAttr "Hand_R_rotateZ.a" "clipLibrary1.cel[0].cev[0].cevr";
connectAttr "Hand_R_rotateY.a" "clipLibrary1.cel[0].cev[1].cevr";
connectAttr "Hand_R_rotateX.a" "clipLibrary1.cel[0].cev[2].cevr";
connectAttr "Hand_R_translateZ.a" "clipLibrary1.cel[0].cev[3].cevr";
connectAttr "Hand_R_translateY.a" "clipLibrary1.cel[0].cev[4].cevr";
connectAttr "Hand_R_translateX.a" "clipLibrary1.cel[0].cev[5].cevr";
connectAttr "Torso_rotateZ.a" "clipLibrary1.cel[0].cev[6].cevr";
connectAttr "Torso_rotateY.a" "clipLibrary1.cel[0].cev[7].cevr";
connectAttr "Torso_rotateX.a" "clipLibrary1.cel[0].cev[8].cevr";
connectAttr "Torso_translateZ.a" "clipLibrary1.cel[0].cev[9].cevr";
connectAttr "Torso_translateY.a" "clipLibrary1.cel[0].cev[10].cevr";
connectAttr "Torso_translateX.a" "clipLibrary1.cel[0].cev[11].cevr";
connectAttr "Hand_L_rotateZ.a" "clipLibrary1.cel[0].cev[12].cevr";
connectAttr "Hand_L_rotateY.a" "clipLibrary1.cel[0].cev[13].cevr";
connectAttr "Hand_L_rotateX.a" "clipLibrary1.cel[0].cev[14].cevr";
connectAttr "Hand_L_translateZ.a" "clipLibrary1.cel[0].cev[15].cevr";
connectAttr "Hand_L_translateY.a" "clipLibrary1.cel[0].cev[16].cevr";
connectAttr "Hand_L_translateX.a" "clipLibrary1.cel[0].cev[17].cevr";
// End of Attack_Cycle.ma
