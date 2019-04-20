/*
Navicat MySQL Data Transfer

Source Server         : database_lbt
Source Server Version : 50561
Source Host           : 127.0.0.1:3306
Source Database       : db_cawc

Target Server Type    : MYSQL
Target Server Version : 50561
File Encoding         : 65001

Date: 2019-04-12 13:08:49
*/

SET FOREIGN_KEY_CHECKS=0;

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
-- Records of user_info
-- ----------------------------
INSERT INTO `user_info` VALUES ('admin', '管理员', 'admin', 'S', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', '2019-04-11 08:59:33');
INSERT INTO `user_info` VALUES ('员工', '员工', '1', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', 'Y', '2019-04-11 10:53:26');
