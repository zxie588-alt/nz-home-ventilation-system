# NZ Home Ventilation System - 中文项目说明

## 项目定位

本项目是一个面向作品集和简历展示的住宅通风系统三维建模项目。模型基于新西兰住宅求职场景，采用概念化三房住宅平面，重点展示 SolidWorks 建模组织能力、机电管线表达能力、参数化脚本生成能力和工程说明能力。

本模型不是施工图、不是合规设计文件，也没有进行风量计算、噪声校核、防火分区或 NZ Building Code 合规审查。它适合作为个人项目中的 CAD/MEP layout concept 展示。

## 模型内容

- 12 m x 8 m 概念住宅平面参考，包括低矮外墙和内墙分隔。
- 一台吊顶服务区内的 MVHR/ERV 主机，带检修面板、过滤抽屉和四个连接端口。
- 蓝色送风系统：主风管、走廊干管、卧室和客餐厅支管、圆形风口。
- 橙色排风系统：厨房、卫生间、洗衣区排风支管和矩形排风格栅。
- 绿色室外新风进气管和灰色外排管，带外墙法兰和简化雨罩。
- 风管连接件：弯头、三通、法兰/卡箍、端部连接环。
- 吊杆和横担支架，用于表现吊顶内安装逻辑。
- 空气流向箭头，用于区分送风、排风、进气和外排方向。
- 顶棚服务区参考轨道，用于表达管路所在的吊顶空间。

## 文件结构

- `01_SolidWorks/NZ_Home_Ventilation_System.SLDPRT`  
  SolidWorks 原生多实体零件文件，包含 151 个实体/特征。

- `02_Exports/NZ_Home_Ventilation_System.step`  
  STEP 中性格式，可用于跨 CAD 软件查看或归档。

- `02_Exports/NZ_Home_Ventilation_System.STL`  
  STL 网格导出，可用于快速预览或上传到展示平台。

- `03_Scripts/BuildNzHomeVentilationSystem.cs`  
  C# 参数化建模源文件，通过 SolidWorks API 创建 BREP 实体。

- `03_Scripts/Build_Model.ps1`  
  一键重新编译并生成模型的 PowerShell 构建脚本。

- `04_Docs/Component_List.csv`  
  自动生成的构件清单，记录特征名、类别、系统、材料/颜色和备注。

- `05_Images/NZ_Home_Ventilation_System_Isometric.png`  
  SolidWorks 导出的着色等轴测预览图。

## 颜色图例

- 蓝色：Supply air，送风管路。
- 橙色：Extract air，厨房、卫生间、洗衣区排风。
- 绿色：Outdoor air intake，室外新风进气。
- 灰色：Exhaust air，外排空气。
- 深灰/黑色：吊杆、支架、检修细节和格栅细节。
- 浅灰：住宅平面和墙体参考。

## 建模假设

- 单位按米建模，输出到 SolidWorks 后可按毫米查看尺寸。
- 住宅平面为概念布局，尺寸约 12 m x 8 m。
- 为了作品集可读性，垂直方向做了展示压缩，风管被放在可见的吊顶服务区高度。
- 主送风/排风管采用约 180 mm 外径表达，支管采用约 120-130 mm 外径表达。
- 风口、格栅、法兰、支架为简化工程表达，重点展示系统构成和建模能力。

## 技术亮点

- 使用 SolidWorks API 自动创建多实体 BREP 模型，而不是单一导入网格。
- 每个风管段、风口、三通、吊杆和设备细节都有英文特征名，便于打开模型时检查结构。
- 通过脚本统一控制管路坐标、半径、颜色、连接件和导出文件。
- 同时输出 SolidWorks 原生文件、STEP、STL、PNG 和构件 CSV，适合放入作品集文件夹。
- 项目展示了从系统布局、模型组织、构件命名、视觉区分到文档交付的完整工作流。

## 可放入简历的英文描述

Residential Ventilation System CAD Model - Personal Project

- Created a 3D residential MVHR/ERV ventilation layout in SolidWorks, including supply and extract ductwork, outdoor intake/exhaust terminals, diffusers, grilles, hangers, collars and equipment details.
- Built the model with a C# SolidWorks API workflow, generating 151 named BREP bodies/features plus STEP, STL, PNG and component-list exports.
- Organised the model by airflow system using colour-coded supply, extract, intake and exhaust routes to communicate MEP layout intent clearly.

## 面试时可以这样介绍

这个项目是我为了展示住宅机电建模能力做的个人作品。我没有户型图，所以先建立了一个新西兰住宅求职场景下比较常见的概念三房布局，然后把 MVHR/ERV 主机、送风管、排风管、室外进排气、风口、支架和连接件做成一个可读的 SolidWorks 多实体模型。项目重点不是施工合规计算，而是展示我能把一个建筑服务系统拆解成清晰的三维构件，并通过参数化脚本保证命名、颜色、导出和清单的一致性。
