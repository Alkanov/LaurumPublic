using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestsCenter : MonoBehaviour
{

    public List<Quest> QuestDatabase;
    public List<Task> TaskDatabase;

    void Awake()
    {
        TaskDatabase = new List<Task>()
        {
new Task(1,Task.task_types.area_explored,new int[] {500},1),//Visit the Bank (Harbour)
new Task(2,Task.task_types.area_explored,new int[] {501},1),//Visit the Potion Shop (Harbour)
new Task(3,Task.task_types.area_explored,new int[] {502},1),//Visit the Skill/Gear Shop (Harbour)
new Task(4,Task.task_types.area_explored,new int[] {503},1),//Enter the Arena
new Task(5,Task.task_types.pve_kill_x_y_times,new int[] {1},10),//Kill 'Slime'
new Task(6,Task.task_types.pve_kill_x_y_times,new int[] {2},10),//Kill 'Worm'
new Task(7,Task.task_types.material_collection,new int[] {4},8),//Collect Slime Core
new Task(8,Task.task_types.material_collection,new int[] {7},8),//Collect Worm
new Task(9,Task.task_types.pve_kill_x_y_times,new int[] {3},15),//Kill 'Bee'
new Task(10,Task.task_types.material_collection,new int[] {9},10),//Collect Honey
new Task(11,Task.task_types.material_collection,new int[] {10},15),//Collect Venom
new Task(12,Task.task_types.pve_kill_x_y_times,new int[] {5},20),//Kill 'Spider'
new Task(13,Task.task_types.material_collection,new int[] {12},25),//Collect Silk Web
new Task(14,Task.task_types.pve_kill_x_y_times,new int[] {6},25),//Kill 'Bat'
new Task(15,Task.task_types.material_collection,new int[] {14},25),//Collect Fruit
new Task(16,Task.task_types.area_explored,new int[] {504},1),//Enter the bat cave
new Task(17,Task.task_types.pve_kill_x_y_times,new int[] {7},25),//Kill 'Blue Mush'
new Task(18,Task.task_types.pve_kill_x_y_times,new int[] {8},25),//Kill 'Red Mush'
new Task(19,Task.task_types.material_collection,new int[] {16},25),//Collect Blue Shroom
new Task(20,Task.task_types.material_collection,new int[] {18},20),//Collect Red Shroom
new Task(21,Task.task_types.pve_kill_x_y_times,new int[] {7},25),//Kill 'Blue Mush'
new Task(22,Task.task_types.pve_kill_x_y_times,new int[] {8},25),//Kill 'Red Mush'
new Task(23,Task.task_types.pve_kill_x_y_times,new int[] {9},25),//Kill 'Zombie'
new Task(24,Task.task_types.material_collection,new int[] {21},20),//Collect Brain
new Task(25,Task.task_types.pve_kill_x_y_times,new int[] {10},25),//Kill 'Ghost'
new Task(26,Task.task_types.material_collection,new int[] {23},15),//Collect Ectoplasm
new Task(27,Task.task_types.material_collection,new int[] {25},25),//Collect Dark Ectoplasm
new Task(28,Task.task_types.pve_kill_x_y_times,new int[] {11},20),//Kill 'Lost Soul'
new Task(29,Task.task_types.pve_kill_x_y_times,new int[] {12},20),//Kill 'Octopus'
new Task(30,Task.task_types.material_collection,new int[] {27},15),//Collect Tentacles
new Task(31,Task.task_types.pve_kill_x_y_times,new int[] {13},25),//Kill 'Water Element'
new Task(32,Task.task_types.material_collection,new int[] {29},25),//Collect Sacred Water
new Task(33,Task.task_types.material_collection,new int[] {31},25),//Collect Blue Staff
new Task(34,Task.task_types.pve_kill_x_y_times,new int[] {15},25),//Kill 'Fire Wizard'
new Task(35,Task.task_types.material_collection,new int[] {35},25),//Collect Wind Stone
new Task(36,Task.task_types.pve_kill_x_y_times,new int[] {17},25),//Kill 'Soulmask'
new Task(37,Task.task_types.material_collection,new int[] {38},10),//Collect Ritual Mask
new Task(38,Task.task_types.material_collection,new int[] {40},20),//Collect Green mushroom
new Task(39,Task.task_types.material_collection,new int[] {42},25),//Collect Ring of protection
new Task(40,Task.task_types.pve_kill_x_y_times,new int[] {18},25),//Kill 'Phasemask'
new Task(41,Task.task_types.pve_kill_x_y_times,new int[] {19},25),//Kill 'Soultalon'
new Task(42,Task.task_types.material_collection,new int[] {41},15),//Collect Heart Stone
new Task(43,Task.task_types.pve_kill_x_y_times,new int[] {20},25),//Kill 'Treant'
new Task(44,Task.task_types.material_collection,new int[] {43},25),//Collect Mahogany Wood
new Task(45,Task.task_types.material_collection,new int[] {44},25),//Collect Leaf
new Task(46,Task.task_types.pve_kill_x_y_times,new int[] {21},25),//Kill 'Golem'
new Task(47,Task.task_types.material_collection,new int[] {45},25),//Collect Black Walnut Wood
new Task(48,Task.task_types.material_collection,new int[] {46},25),//Collect Autumn Leaf
new Task(49,Task.task_types.pve_kill_x_y_times,new int[] {22},25),//Kill 'Scorpion'
new Task(50,Task.task_types.material_collection,new int[] {47},25),//Collect Scorpion Tail
new Task(51,Task.task_types.material_collection,new int[] {48},25),//Collect Scorpion
new Task(52,Task.task_types.pve_kill_x_y_times,new int[] {23},25),//Kill 'Sand Guard'
new Task(53,Task.task_types.material_collection,new int[] {49},25),//Collect Earth Stone
new Task(54,Task.task_types.material_collection,new int[] {50},25),//Collect Enchanted Sand
new Task(55,Task.task_types.pve_kill_x_y_times,new int[] {24},25),//Kill 'Skeleton'
new Task(56,Task.task_types.material_collection,new int[] {51},25),//Collect Dusty Tome
new Task(57,Task.task_types.material_collection,new int[] {52},25),//Collect Gold Tooth
new Task(58,Task.task_types.pve_kill_x_y_times,new int[] {25},25),//Kill 'Skeleton Chief'
new Task(59,Task.task_types.material_collection,new int[] {53},25),//Collect Ancient Axe
new Task(60,Task.task_types.material_collection,new int[] {54},25),//Collect Golden Horn
new Task(61,Task.task_types.area_explored,new int[] {505},1),//Explore desert entrance A
new Task(62,Task.task_types.area_explored,new int[] {506},1),//Explore desert entrance B
new Task(63,Task.task_types.area_explored,new int[] {507},1),//Explore fire cave 
new Task(64,Task.task_types.area_explored,new int[] {508},1),//Explore fire cave boss A
new Task(65,Task.task_types.area_explored,new int[] {509},1),//Explore fire cave boss B
new Task(66,Task.task_types.pve_kill_x_y_times,new int[] {26},25),//Kill 'Fire Lich'
new Task(67,Task.task_types.material_collection,new int[] {55},25),//Collect Fire Bracelet
new Task(68,Task.task_types.material_collection,new int[] {56},25),//Collect Fire Orb
new Task(69,Task.task_types.pve_kill_x_y_times,new int[] {27},25),//Kill 'Fire Skeleton'
new Task(70,Task.task_types.material_collection,new int[] {57},25),//Collect Fire Stone
new Task(71,Task.task_types.material_collection,new int[] {58},25),//Collect Charred Bone
new Task(72,Task.task_types.pve_kill_x_y_times,new int[] {28},25),//Kill 'Death Watch'
new Task(73,Task.task_types.material_collection,new int[] {59},25),//Collect Magic Eyeball
new Task(74,Task.task_types.material_collection,new int[] {60},25),//Collect Magic Leather
new Task(75,Task.task_types.pve_kill_x_y_times,new int[] {29},25),//Kill 'Observer'
new Task(76,Task.task_types.material_collection,new int[] {61},25),//Collect Eyeball
new Task(77,Task.task_types.material_collection,new int[] {62},25),//Collect Leather
new Task(78,Task.task_types.area_explored,new int[] {510},1),//Enter Rynthia portal
new Task(79,Task.task_types.area_explored,new int[] {511},1),//Find the Princess camp
new Task(80,Task.task_types.pve_kill_x_y_times,new int[] {30},25),//Kill 'Ice Lich'
new Task(81,Task.task_types.material_collection,new int[] {63},25),//Collect Ice Bracelet
new Task(82,Task.task_types.material_collection,new int[] {64},25),//Collect Regal Crown
new Task(83,Task.task_types.pve_kill_x_y_times,new int[] {31},25),//Kill 'Ice Skeleton'
new Task(84,Task.task_types.material_collection,new int[] {65},25),//Collect Purple Cape
new Task(85,Task.task_types.material_collection,new int[] {66},25),//Collect Heart of Ice skeleton
new Task(86,Task.task_types.ds_enter,new int[] {100},2),//Enter Devil Square
new Task(87,Task.task_types.ds_pve_kill,new int[] {100},25),//Kill 25 enemies in D.S
new Task(88,Task.task_types.arena_pvp_kill,new int[] {0},20),//Take down 20 players in the Arena
new Task(89,Task.task_types.ds_pve_kill,new int[] {150},5),//Kill 5 enemies in D.S
new Task(90,Task.task_types.ds_wave,new int[] {150},5),//Reach wave 5 in D.S
new Task(91,Task.task_types.ds_pve_kill,new int[] {150},12),//Kill 12 enemies in D.S
new Task(92,Task.task_types.ds_wave,new int[] {150},5),//Reach wave 5 in D.S
new Task(93,Task.task_types.ds_pve_kill,new int[] {150},25),//Kill 25 enemies in D.S
new Task(94,Task.task_types.ds_wave,new int[] {150},10),//Reach wave 10 in D.S
new Task(95,Task.task_types.ds_pve_kill,new int[] {150},40),//Kill 40 enemies in D.S
new Task(96,Task.task_types.ds_wave,new int[] {150},15),//Reach wave 15 in D.S
new Task(97,Task.task_types.ds_pve_kill,new int[] {150},60),//Kill 60 enemies in D.S
new Task(98,Task.task_types.ds_wave,new int[] {150},20),//Reach wave 20 in D.S
new Task(99,Task.task_types.ds_pve_kill,new int[] {150},80),//Kill 80 enemies in D.S
new Task(100,Task.task_types.ds_wave,new int[] {150},25),//Reach wave 25 in D.S
new Task(101,Task.task_types.ds_pve_kill,new int[] {150},100),//Kill 100 enemies in D.S
new Task(102,Task.task_types.ds_wave,new int[] {150},30),//Reach wave 30 in D.S
new Task(103,Task.task_types.ds_pve_kill,new int[] {150},120),//Kill 120 enemies in D.S
new Task(104,Task.task_types.ds_wave,new int[] {150},30),//Reach wave 30 in D.S
new Task(105,Task.task_types.ds_pve_kill,new int[] {150},140),//Kill 140 enemies in D.S
new Task(106,Task.task_types.ds_wave,new int[] {150},30),//Reach wave 30 in D.S
new Task(107,Task.task_types.ds_pve_kill,new int[] {40},20),//Kill 20 enemies in D.S
new Task(108,Task.task_types.pve_kill_x_y_times,new int[] {58},1),//Kill 'Alainon the Bane'
new Task(109,Task.task_types.pve_kill_x_y_times,new int[] {57},1),//Kill 'Abelot the Keeper'
new Task(110,Task.task_types.material_collection,new int[] {61},25),//Collect Eyeball
new Task(111,Task.task_types.material_collection,new int[] {54},15),//Collect Golden Horn
new Task(112,Task.task_types.material_collection,new int[] {50},15),//Collect Enchanted Sand
new Task(113,Task.task_types.material_collection,new int[] {66},15),//Collect Heart of Ice skeleton
new Task(114,Task.task_types.online_time,new int[] {0},600),//Stay Online for 10 minutes
new Task(115,Task.task_types.online_time,new int[] {0},43200),//Stay Online for 12 Hours
new Task(116,Task.task_types.pve_kill_x_y_times,new int[] {20},500),//Kill 'Treant'
new Task(117,Task.task_types.pve_kill_x_y_times,new int[] {21},500),//Kill 'Golem'
new Task(118,Task.task_types.pve_kill_x_y_times,new int[] {28},750),//Kill 'Death Watch'
new Task(119,Task.task_types.pve_kill_x_y_times,new int[] {29},750),//Kill 'Observer'
new Task(120,Task.task_types.pve_kill_x_y_times,new int[] {30},1000),//Kill 'Ice Lich'
new Task(121,Task.task_types.pve_kill_x_y_times,new int[] {31},1000),//Kill 'Ice Skeleton'
new Task(122,Task.task_types.pvp_kill,new int[] {0},10),//10 Open World PvP Kills
        };

        QuestDatabase = new List<Quest>() {
new Quest(1,1,Quest.quest_types.alone_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),1,5,2138,1000,null,57,new List<Task>(){FetchTaskByID(1),FetchTaskByID(2),FetchTaskByID(3),FetchTaskByID(4)}),
new Quest(2,1,Quest.quest_types.alone_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),1,5,800,1000,null,58,new List<Task>(){FetchTaskByID(5),FetchTaskByID(6)}),
new Quest(3,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),1,5,1700,1000,null,0,new List<Task>(){FetchTaskByID(7),FetchTaskByID(8)}),
new Quest(4,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),3,8,1900,1200,null,0,new List<Task>(){FetchTaskByID(9)}),
new Quest(5,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),4,11,2100,1500,null,0,new List<Task>(){FetchTaskByID(10),FetchTaskByID(11)}),
new Quest(6,1,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),5,13,1500,2000,null,0,new List<Task>(){FetchTaskByID(16)}),
new Quest(7,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),6,15,2500,2200,null,0,new List<Task>(){FetchTaskByID(12)}),
new Quest(8,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),6,15,3000,2200,null,0,new List<Task>(){FetchTaskByID(13)}),
new Quest(9,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),6,15,3500,2200,null,0,new List<Task>(){FetchTaskByID(14)}),
new Quest(10,1,Quest.quest_types.alone_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),6,17,3800,2200,null,0,new List<Task>(){FetchTaskByID(15)}),
new Quest(11,2,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),10,20,4500,3000,null,0,new List<Task>(){FetchTaskByID(17),FetchTaskByID(18)}),
new Quest(12,2,Quest.quest_types.alone_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),10,20,6500,3000,null,0,new List<Task>(){FetchTaskByID(19)}),
new Quest(13,2,Quest.quest_types.alone_quest,true,-1,12,new Quest.QuestConditions(-1,false,false,false),10,20,8000,3000,null,0,new List<Task>(){FetchTaskByID(20)}),
new Quest(14,2,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(30,false,false,false),10,20,12000,3000,null,0,new List<Task>(){FetchTaskByID(21),FetchTaskByID(22)}),
new Quest(15,3,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(5,true,false,false),15,23,12000,4000,null,0,new List<Task>(){FetchTaskByID(23)}),
new Quest(16,3,Quest.quest_types.alone_quest,true,60,15,new Quest.QuestConditions(-1,true,false,false),15,25,10000,4000,null,0,new List<Task>(){FetchTaskByID(24)}),
new Quest(17,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),18,27,13750,4600,null,0,new List<Task>(){FetchTaskByID(25),FetchTaskByID(26)}),
new Quest(18,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),20,28,14745,5000,null,0,new List<Task>(){FetchTaskByID(27),FetchTaskByID(28)}),
new Quest(19,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,true,false,false),22,28,15740,5200,null,0,new List<Task>(){FetchTaskByID(25),FetchTaskByID(26),FetchTaskByID(27),FetchTaskByID(28)}),
new Quest(20,1,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),25,35,16735,6000,null,0,new List<Task>(){FetchTaskByID(29),FetchTaskByID(30)}),
new Quest(21,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),25,35,17729,6000,null,0,new List<Task>(){FetchTaskByID(31),FetchTaskByID(32)}),
new Quest(22,1,Quest.quest_types.party_allowed_quest,true,60,20,new Quest.QuestConditions(-1,false,false,false),25,35,18724,6000,null,0,new List<Task>(){FetchTaskByID(29),FetchTaskByID(30),FetchTaskByID(31),FetchTaskByID(32)}),
new Quest(23,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),30,38,19719,7000,null,0,new List<Task>(){FetchTaskByID(33),FetchTaskByID(34)}),
new Quest(24,1,Quest.quest_types.alone_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),35,45,20714,8000,null,0,new List<Task>(){FetchTaskByID(35)}),
new Quest(25,1,Quest.quest_types.alone_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),38,48,21709,8600,null,0,new List<Task>(){FetchTaskByID(36),FetchTaskByID(37)}),
new Quest(26,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),40,48,22704,9000,null,0,new List<Task>(){FetchTaskByID(38),FetchTaskByID(39)}),
new Quest(27,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),40,48,23698,9000,null,0,new List<Task>(){FetchTaskByID(40),FetchTaskByID(41),FetchTaskByID(42)}),
new Quest(28,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),52,62,24693,11300,null,0,new List<Task>(){FetchTaskByID(43)}),
new Quest(29,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),52,62,25688,11300,null,0,new List<Task>(){FetchTaskByID(45)}),
new Quest(30,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),52,62,26683,11300,null,0,new List<Task>(){FetchTaskByID(46)}),
new Quest(31,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),52,62,27678,11300,null,0,new List<Task>(){FetchTaskByID(47)}),
new Quest(32,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),52,60,28673,11300,null,0,new List<Task>(){FetchTaskByID(44),FetchTaskByID(48)}),
new Quest(33,4,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),60,72,29667,13000,null,0,new List<Task>(){FetchTaskByID(49)}),
new Quest(34,4,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),60,72,30662,13000,null,0,new List<Task>(){FetchTaskByID(50)}),
new Quest(35,4,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),60,72,31657,13000,null,0,new List<Task>(){FetchTaskByID(51)}),
new Quest(36,4,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),65,75,32652,14000,null,0,new List<Task>(){FetchTaskByID(52)}),
new Quest(37,4,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),65,75,33647,14000,null,0,new List<Task>(){FetchTaskByID(53)}),
new Quest(38,4,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),65,75,34642,14000,null,0,new List<Task>(){FetchTaskByID(54)}),
new Quest(39,4,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),70,84,35637,15000,null,0,new List<Task>(){FetchTaskByID(61),FetchTaskByID(62)}),
new Quest(40,5,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),72,84,36631,15300,null,0,new List<Task>(){FetchTaskByID(55),FetchTaskByID(58)}),
new Quest(41,5,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),72,84,37626,15300,null,0,new List<Task>(){FetchTaskByID(57),FetchTaskByID(59)}),
new Quest(42,4,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),80,95,38621,17000,null,0,new List<Task>(){FetchTaskByID(63),FetchTaskByID(64),FetchTaskByID(65)}),
new Quest(43,6,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),80,95,39616,17000,null,0,new List<Task>(){FetchTaskByID(66),FetchTaskByID(69)}),
new Quest(44,6,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),80,95,40611,17000,null,0,new List<Task>(){FetchTaskByID(67),FetchTaskByID(71)}),
new Quest(45,1,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),90,120,41606,19000,null,0,new List<Task>(){FetchTaskByID(72),FetchTaskByID(75),FetchTaskByID(73)}),
new Quest(46,1,Quest.quest_types.party_allowed_quest,true,-1,45,new Quest.QuestConditions(-1,false,false,false),90,120,42600,19000,null,0,new List<Task>(){FetchTaskByID(74),FetchTaskByID(76)}),
new Quest(47,1,Quest.quest_types.party_allowed_quest,true,-1,46,new Quest.QuestConditions(-1,false,false,false),90,130,43595,19000,null,0,new List<Task>(){FetchTaskByID(77)}),
new Quest(48,1,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),90,150,44350,19000,null,0,new List<Task>(){FetchTaskByID(72),FetchTaskByID( 75)}),
new Quest(49,1,Quest.quest_types.party_allowed_quest,true,-1,47,new Quest.QuestConditions(-1,false,false,false),100,150,44590,20000,null,0,new List<Task>(){FetchTaskByID(78)}),
new Quest(50,7,Quest.quest_types.party_allowed_quest,true,-1,-1,new Quest.QuestConditions(-1,false,false,false),100,250,45585,20000,null,0,new List<Task>(){FetchTaskByID(79)}),
new Quest(51,7,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),100,250,46580,20000,null,0,new List<Task>(){FetchTaskByID(80),FetchTaskByID(83)}),
new Quest(52,8,Quest.quest_types.party_allowed_quest,true,60,-1,new Quest.QuestConditions(-1,false,false,false),100,250,48569,20000,null,0,new List<Task>(){FetchTaskByID(81),FetchTaskByID(82),FetchTaskByID(84),FetchTaskByID(85)}),
new Quest(53,1,Quest.quest_types.party_allowed_quest,false,30,-1,new Quest.QuestConditions(-1,false,false,false),100,250,38000,20000,null,0,new List<Task>(){FetchTaskByID(86),FetchTaskByID(87)}),
new Quest(54,1,Quest.quest_types.alone_quest,false,30,-1,new Quest.QuestConditions(-1,false,false,false),100,250,35140,20000,null,0,new List<Task>(){FetchTaskByID(88)}),
new Quest(55,1,Quest.quest_types.alone_quest,true,20,-1,new Quest.QuestConditions(-1,true,false,false),3,30,2685,0,null,0,new List<Task>(){FetchTaskByID(114)}),
new Quest(56,1,Quest.quest_types.alone_quest,true,20,-1,new Quest.QuestConditions(-1,true,false,false),31,60,3580,0,null,0,new List<Task>(){FetchTaskByID(114)}),
new Quest(57,1,Quest.quest_types.alone_quest,true,20,-1,new Quest.QuestConditions(-1,true,false,false),61,90,4475,0,null,0,new List<Task>(){FetchTaskByID(114)}),
new Quest(58,1,Quest.quest_types.alone_quest,true,20,-1,new Quest.QuestConditions(-1,true,false,false),91,120,5370,0,null,0,new List<Task>(){FetchTaskByID(114)}),
new Quest(59,1,Quest.quest_types.alone_quest,true,20,-1,new Quest.QuestConditions(-1,true,false,false),121,150,6265,0,null,0,new List<Task>(){FetchTaskByID(114)}),



new Quest(500,1,Quest.quest_types.alone_quest,false,1440,-1,new Quest.QuestConditions(-1,false,false,false),10,250,0,0,new List<int>(){4002},0,new List<Task>(){FetchTaskByID(88)}),
new Quest(501,1,Quest.quest_types.alone_quest,false,1440,-1,new Quest.QuestConditions(-1,false,false,false),50,250,0,0,new List<int>(){4002},0,new List<Task>(){FetchTaskByID(107)}),







new Quest(601,1,Quest.quest_types.alone_quest,false,-1,-1,new Quest.QuestConditions(30,false,false,false),150,250,28000,5500,null,59,new List<Task>(){FetchTaskByID(89),FetchTaskByID(90)}),
new Quest(602,1,Quest.quest_types.alone_quest,false,-1,601,new Quest.QuestConditions(30,false,false,false),150,250,38000,15000,null,60,new List<Task>(){FetchTaskByID(91),FetchTaskByID(92)}),
new Quest(603,1,Quest.quest_types.alone_quest,false,-1,602,new Quest.QuestConditions(30,false,false,false),150,250,48000,24500,null,61,new List<Task>(){FetchTaskByID(93),FetchTaskByID(94)}),
new Quest(604,1,Quest.quest_types.alone_quest,false,-1,603,new Quest.QuestConditions(30,false,false,false),150,250,58000,34000,null,62,new List<Task>(){FetchTaskByID(95),FetchTaskByID(96)}),
new Quest(605,1,Quest.quest_types.alone_quest,false,-1,604,new Quest.QuestConditions(30,false,false,false),150,250,68000,43500,null,63,new List<Task>(){FetchTaskByID(97),FetchTaskByID(98)}),
new Quest(606,1,Quest.quest_types.alone_quest,false,-1,605,new Quest.QuestConditions(30,false,false,false),150,250,78000,53000,null,64,new List<Task>(){FetchTaskByID(99),FetchTaskByID(100)}),
new Quest(607,1,Quest.quest_types.alone_quest,false,-1,606,new Quest.QuestConditions(30,false,false,false),150,250,88000,62500,null,65,new List<Task>(){FetchTaskByID(101),FetchTaskByID(102)}),
new Quest(608,1,Quest.quest_types.alone_quest,false,-1,607,new Quest.QuestConditions(30,false,true,false),150,250,98000,72000,null,66,new List<Task>(){FetchTaskByID(103),FetchTaskByID(104)}),
new Quest(609,1,Quest.quest_types.alone_quest,false,-1,608,new Quest.QuestConditions(30,false,false,true),150,250,108000,81500,null,67,new List<Task>(){FetchTaskByID(105),FetchTaskByID(106)}),

new Quest(300,4,Quest.quest_types.party_allowed_quest,false,-1,49,new Quest.QuestConditions(-1,false,false,false),100,150,35000,5500,null,0,new List<Task>(){FetchTaskByID(66),FetchTaskByID( 69)}),
new Quest(301,4,Quest.quest_types.party_allowed_quest,false,-1,300,new Quest.QuestConditions(-1,false,false,false),100,200,50000,45000,null,0,new List<Task>(){FetchTaskByID(108)}),
new Quest(302,4,Quest.quest_types.party_allowed_quest,false,-1,49,new Quest.QuestConditions(-1,false,false,false),100,150,35000,5500,null,0,new List<Task>(){FetchTaskByID(110),FetchTaskByID( 111),FetchTaskByID( 112),FetchTaskByID( 113)}),
new Quest(303,4,Quest.quest_types.party_allowed_quest,false,-1,302,new Quest.QuestConditions(-1,false,false,false),100,200,50000,45000,null,0,new List<Task>(){FetchTaskByID(109)}),






//new Quest(900,6,Quest.quest_types.party_allowed_quest,false,1440,-1,new Quest.QuestConditions(-1,true,false,false),150,250,0,0,new List<int>(){4003},0,new List<Task>(){FetchTaskByID(108),FetchTaskByID(109)}),
new Quest(901,4,Quest.quest_types.party_allowed_quest,false,10080,-1,new Quest.QuestConditions(10080,false,false,false),50,75,100000,50000,new List<int>(){4002},0,new List<Task>(){FetchTaskByID(115),FetchTaskByID(116),FetchTaskByID( 117)}),
new Quest(902,1,Quest.quest_types.party_allowed_quest,false,10080,-1,new Quest.QuestConditions(10080,false,false,false),80,105,200000,50000,new List<int>(){4002},0,new List<Task>(){FetchTaskByID(115),FetchTaskByID(118),FetchTaskByID(119)}),
new Quest(903,7,Quest.quest_types.party_allowed_quest,false,10080,-1,new Quest.QuestConditions(10080,false,false,false),110,150,300000,0,new List<int>(){4000},0,new List<Task>(){FetchTaskByID(115),FetchTaskByID(120),FetchTaskByID(121)}),
new Quest(904,1,Quest.quest_types.alone_quest,false,1440,-1,new Quest.QuestConditions(-1,true,false,false),10,150,50000,25000,null,0,new List<Task>(){FetchTaskByID(122)}),

        };
        /*Debug.Log(JsonHelper.ToJson(QuestDatabase.ToArray()));

        playerQuestLogs = new List<QuestLogs>()//Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        {
            new QuestLogs(1,(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,new List<int>(){20,35}, QuestLogs.status.done)
        };
        string toload = JsonHelper.ToJson(playerQuestLogs.ToArray());
        Debug.Log(JsonHelper.ToJson(playerQuestLogs.ToArray()));
        playerQuestLogs_loaded = new List<QuestLogs>( JsonHelper.FromJson<QuestLogs>(toload) );*/
    }




    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    public Task FetchTaskByID(int ID)
    {
        for (int i = 0; i < TaskDatabase.Count; i++)
            if (TaskDatabase[i].task_id == ID)
                return TaskDatabase[i];
        return null;
    }
    public Quest FetchQuestByID(int ID)
    {
        for (int i = 0; i < QuestDatabase.Count; i++)
            if (QuestDatabase[i]._quest_ID == ID)
                return QuestDatabase[i];
        return null;
    }
    public List<Quest> FetchQuestByNPC(int NPCID)
    {
        var questfound = new List<Quest>();
        for (int i = 0; i < QuestDatabase.Count; i++)
        {
            if (QuestDatabase[i].given_by_this_NPC_ID == NPCID || NPCID == -100)
            {
                questfound.Add(QuestDatabase[i]);
            }

        }
        return questfound;
    }
}
