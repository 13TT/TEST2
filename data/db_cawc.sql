/*
Navicat MySQL Data Transfer

Source Server         : database_lbt
Source Server Version : 50561
Source Host           : 127.0.0.1:3306
Source Database       : db_cawc

Target Server Type    : MYSQL
Target Server Version : 50561
File Encoding         : 65001

Date: 2019-04-12 13:08:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for barcodeprint
-- ----------------------------
DROP TABLE IF EXISTS `barcodeprint`;
CREATE TABLE `barcodeprint` (
  `BarCodeID` int(9) NOT NULL AUTO_INCREMENT,
  `BarcodeString` varchar(100) DEFAULT NULL,
  `Goods_Code` varchar(20) DEFAULT NULL,
  `Goods_Name` varchar(20) DEFAULT NULL,
  `InstoreDate` varchar(20) DEFAULT NULL,
  `Status` int(2) DEFAULT '0',
  PRIMARY KEY (`BarCodeID`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for check_info
-- ----------------------------
DROP TABLE IF EXISTS `check_info`;
CREATE TABLE `check_info` (
  `check_id` varchar(255) NOT NULL,
  `house_number` varchar(255) NOT NULL,
  `goods_code` varchar(255) NOT NULL,
  PRIMARY KEY (`check_id`,`house_number`,`goods_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for complete_info
-- ----------------------------
DROP TABLE IF EXISTS `complete_info`;
CREATE TABLE `complete_info` (
  `order_id` int(11) NOT NULL DEFAULT '0',
  `goods_code` varchar(255) NOT NULL DEFAULT '',
  `complete_in_number` int(255) DEFAULT '0',
  `complete_out_number` int(255) DEFAULT '0',
  `current_number` int(255) DEFAULT '0',
  `compelte_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`order_id`,`goods_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for config_doc_style
-- ----------------------------
DROP TABLE IF EXISTS `config_doc_style`;
CREATE TABLE `config_doc_style` (
  `unit_code` varchar(255) NOT NULL,
  `unit_name` varchar(255) DEFAULT '',
  PRIMARY KEY (`unit_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for config_goods_differ
-- ----------------------------
DROP TABLE IF EXISTS `config_goods_differ`;
CREATE TABLE `config_goods_differ` (
  `unit_code` varchar(255) NOT NULL,
  `unit_name` varchar(255) DEFAULT '',
  PRIMARY KEY (`unit_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for config_goods_style
-- ----------------------------
DROP TABLE IF EXISTS `config_goods_style`;
CREATE TABLE `config_goods_style` (
  `unit_code` varchar(255) NOT NULL,
  `unit_name` varchar(255) DEFAULT '',
  PRIMARY KEY (`unit_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for config_goods_unit
-- ----------------------------
DROP TABLE IF EXISTS `config_goods_unit`;
CREATE TABLE `config_goods_unit` (
  `unit_code` varchar(255) NOT NULL,
  `unit_name` varchar(255) DEFAULT '',
  PRIMARY KEY (`unit_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for config_unit
-- ----------------------------
DROP TABLE IF EXISTS `config_unit`;
CREATE TABLE `config_unit` (
  `unit_name` varchar(255) NOT NULL DEFAULT '',
  `table_name` varchar(255) DEFAULT '',
  PRIMARY KEY (`unit_name`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for doc_data
-- ----------------------------
DROP TABLE IF EXISTS `doc_data`;
CREATE TABLE `doc_data` (
  `doc_id` varchar(255) NOT NULL DEFAULT '',
  `goods_code` varchar(255) NOT NULL,
  `plan_number` int(255) DEFAULT '0',
  `order_number` int(255) DEFAULT '0',
  PRIMARY KEY (`doc_id`,`goods_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for doc_info
-- ----------------------------
DROP TABLE IF EXISTS `doc_info`;
CREATE TABLE `doc_info` (
  `doc_id` varchar(255) NOT NULL,
  `doc_style` varchar(255) DEFAULT '',
  `send_user` varchar(255) DEFAULT '',
  `rec_user` varchar(255) DEFAULT '',
  `doc_time` date DEFAULT '2016-03-08',
  PRIMARY KEY (`doc_id`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for goods_info
-- ----------------------------
DROP TABLE IF EXISTS `goods_info`;
CREATE TABLE `goods_info` (
  `goods_id` int(11) NOT NULL AUTO_INCREMENT,
  `goods_code` varchar(255) DEFAULT NULL,
  `goods_name` varchar(255) DEFAULT '',
  `goods_height` varchar(255) DEFAULT '' COMMENT '规格',
  `goods_differ` varchar(255) DEFAULT '',
  `goods_style` varchar(255) DEFAULT '',
  `goods_unit` varchar(255) DEFAULT '',
  `house_style` char(1) DEFAULT 'A',
  `unit_weight` varchar(255) DEFAULT '',
  `modify_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`goods_id`)
) ENGINE=InnoDB AUTO_INCREMENT=201 DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for house_data
-- ----------------------------
DROP TABLE IF EXISTS `house_data`;
CREATE TABLE `house_data` (
  `house_number` varchar(255) NOT NULL,
  `goods_code` varchar(255) NOT NULL,
  `goods_name` varchar(255) DEFAULT '',
  `in_house_time` datetime DEFAULT '2016-03-16 00:00:00',
  `have_number` int(255) DEFAULT '0',
  `plan_in_number` int(255) DEFAULT '0',
  `bf_plan_in_number` int(255) DEFAULT '0',
  `plan_out_number` int(255) DEFAULT '0',
  `bf_plan_out_number` int(255) DEFAULT '0',
  `Vendor` varchar(255) DEFAULT NULL,
  `PacketSeqNo` varchar(255) DEFAULT NULL,
  `BarCode` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`house_number`,`goods_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for house_info
-- ----------------------------
DROP TABLE IF EXISTS `house_info`;
CREATE TABLE `house_info` (
  `house_number` varchar(255) NOT NULL,
  `house_state` char(1) DEFAULT 'N',
  `bf_h_to_h` varchar(10) DEFAULT '',
  `bf_h_to_h_2` char(1) DEFAULT '',
  `before_change` char(1) DEFAULT '',
  `house_style` varchar(10) DEFAULT 'A',
  `line` tinyint(255) DEFAULT '0',
  `list` tinyint(255) DEFAULT '0',
  `blank` tinyint(255) DEFAULT '0',
  `floor` tinyint(255) DEFAULT '0',
  `modify_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`house_number`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for order_info
-- ----------------------------
DROP TABLE IF EXISTS `order_info`;
CREATE TABLE `order_info` (
  `order_id` int(255) unsigned zerofill NOT NULL AUTO_INCREMENT,
  `order_state` smallint(255) NOT NULL DEFAULT '0' COMMENT '0-尚未处理,1-正在处理，2-完成处理',
  `order_model` smallint(255) DEFAULT '0',
  `order_level` smallint(255) NOT NULL DEFAULT '0' COMMENT '0-最低等级',
  `order_style` smallint(255) NOT NULL DEFAULT '0' COMMENT '1-入库  2-出库  3-减料 4-加料 5-站对站 6-库对库 7-移动 8-盘点',
  `station_from` varchar(255) DEFAULT '0' COMMENT '101-1号线01号站口，102-1号线02号站口',
  `station_to` varchar(255) DEFAULT '0',
  `house_from` varchar(255) DEFAULT '0',
  `house_to` varchar(255) DEFAULT '0',
  `order_user` varchar(255) DEFAULT '',
  `doc_id` varchar(255) DEFAULT '',
  `check_id` varchar(255) DEFAULT '',
  `order_time` datetime DEFAULT '2016-03-16 00:00:00',
  `Line` smallint(2) DEFAULT NULL,
  PRIMARY KEY (`order_id`)
) ENGINE=InnoDB AUTO_INCREMENT=321 DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for plc_data
-- ----------------------------
DROP TABLE IF EXISTS `plc_data`;
CREATE TABLE `plc_data` (
  `line` int(255) NOT NULL,
  `plc_walk` int(255) NOT NULL DEFAULT '0',
  `plc_order_id` int(255) DEFAULT '0',
  `plc_order_style` int(255) DEFAULT '0',
  `plc_station_from` int(255) DEFAULT NULL,
  `plc_station_to` int(255) DEFAULT NULL,
  `plc_list_from` int(255) DEFAULT '0',
  `plc_blank_from` int(255) DEFAULT '0',
  `plc_floor_from` int(255) DEFAULT '0',
  `plc_list_to` int(255) DEFAULT '0',
  `plc_blank_to` int(255) DEFAULT '0',
  `plc_floor_to` int(255) DEFAULT '0',
  `plc_order_state` int(255) DEFAULT '0',
  PRIMARY KEY (`line`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- Table structure for user_info
-- ----------------------------
DROP TABLE IF EXISTS `user_info`;
CREATE TABLE `user_info` (
  `user_code` varchar(255) NOT NULL,
  `user_name` varchar(255) NOT NULL,
  `pass_word` varchar(255) NOT NULL,
  `use_pl_new_pallet` char(1) DEFAULT 'N',
  `use_pl_add_pallet` char(1) DEFAULT 'N',
  `use_pl_pack_in` char(1) DEFAULT 'N',
  `use_pl_goods_out` char(1) DEFAULT 'N',
  `use_pl_house_out` char(1) DEFAULT 'N',
  `use_pl_pack_out` char(1) DEFAULT 'N',
  `use_pl_empty_pallet_in` char(1) DEFAULT 'N',
  `use_pl_empty_pallet_out` char(1) DEFAULT 'N',
  `use_pl_S_to_S` char(1) DEFAULT 'N',
  `use_pl_H_to_H` char(1) DEFAULT 'N',
  `use_mt_order` char(1) DEFAULT 'N',
  `use_mt_goods` char(1) DEFAULT 'N',
  `use_mt_house_data` char(1) DEFAULT 'N',
  `use_mt_house_sts` char(1) DEFAULT 'N',
  `use_mt_doc` char(1) DEFAULT 'N',
  `use_mt_unit` char(1) DEFAULT 'N',
  `use_mt_use` char(1) DEFAULT 'N',
  `use_mt_user_info` char(1) DEFAULT 'N',
  `use_qy_order` char(1) DEFAULT 'N',
  `use_qy_station` char(1) DEFAULT 'N',
  `use_qy_goods` char(1) DEFAULT 'N',
  `use_qy_house_data` char(1) DEFAULT 'N',
  `use_qy_house_state` char(1) DEFAULT 'N',
  `use_qy_stock_data` char(1) DEFAULT 'N',
  `use_qy_doc_data` char(1) DEFAULT 'N',
  `use_qy_stacker` char(1) DEFAULT 'N',
  `use_ck_goods` char(1) DEFAULT 'N',
  `use_ck_house` char(1) DEFAULT 'N',
  `use_ck_data_modify` char(1) DEFAULT 'N',
  `use_rp_day_month` char(1) DEFAULT 'N',
  `use_rp_house_data` char(1) DEFAULT 'N',
  `use_ss_back_up` char(1) DEFAULT 'N',
  `use_ss_restore` char(1) DEFAULT 'N',
  `use_ss_remove_old` char(1) DEFAULT 'N',
  `modify_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_code`)
) ENGINE=InnoDB DEFAULT CHARSET=gbk;

-- ----------------------------
-- View structure for v_good_info
-- ----------------------------
DROP VIEW IF EXISTS `v_good_info`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `v_good_info` AS select concat(`goods_info`.`goods_code`,';',`goods_info`.`goods_name`,';',sysdate()) AS `barcode_QS`,`goods_info`.`goods_code` AS `goods_code`,`goods_info`.`goods_name` AS `goods_name`,sysdate() AS `instoredate` from `goods_info` ;
