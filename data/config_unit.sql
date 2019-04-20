/*
Navicat MySQL Data Transfer

Source Server         : database_lbt
Source Server Version : 50561
Source Host           : 127.0.0.1:3306
Source Database       : db_cawc

Target Server Type    : MYSQL
Target Server Version : 50561
File Encoding         : 65001

Date: 2019-04-14 09:06:13
*/

SET FOREIGN_KEY_CHECKS=0;

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
-- Records of config_unit
-- ----------------------------
INSERT INTO `config_unit` VALUES ('S', 'admin');
