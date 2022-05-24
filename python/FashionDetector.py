# YOLOv5 🚀 by Ultralytics, GPL-3.0 license
"""
Run inference on images, videos, directories, streams, etc.

Usage:
    $ python path/to/detect.py --weights yolov5s.pt --source 0  # webcam
                                                             img.jpg  # image
                                                             vid.mp4  # video
                                                             path/  # directory
                                                             path/*.jpg  # glob
                                                             'https://youtu.be/Zgi9g1ksQHc'  # YouTube
                                                             'rtsp://example.com/media.mp4'  # RTSP, RTMP, HTTP stream
"""
import argparse
import sys

from pathlib import Path

import cv2
import torch.backends.cudnn as cudnn

FILE = Path(__file__).resolve()
ROOT = FILE.parents[1] #Root Dircetory ~~/ProjectMain/
YOLO = Path(FILE.parents[0].__str__() + "/yolov5") #ProjectMain/python/yolov5/
WEIGHT = Path(FILE.parents[1].__str__() + "/weight")

from yolov5.models.common import DetectMultiBackend
from yolov5.utils.dataloaders import IMG_FORMATS, VID_FORMATS, LoadImages, LoadStreams
from yolov5.utils.general import (LOGGER, check_file, check_img_size, check_imshow, check_requirements, colorstr, cv2,
                           increment_path, non_max_suppression, print_args, scale_coords, strip_optimizer, xyxy2xywh)
from yolov5.utils.plots import Annotator, colors, save_one_box
from yolov5.utils.torch_utils import select_device, time_sync


from PIL import Image
import torch
import numpy as np
import python.ClothesObject

class FashionObjectDetector:
    object_class = ['overall', 'bottom', 'top', 'outer', 'shoes']
    device = "cuda:0"
    conf_thres = 0.4
    iou_thres = 0.45
    max_det = 1000
    line_thickness = 3

    def __init__(self, dev_value, model_path):
        self.set_dev = dev_value
        self.model_path = model_path

    # 디바이스 설정 함수
    def set_device(self):
        if self.set_dev == 0:
            self.selected_device = select_device(self.device)
        else:
            device = torch.device("cpu")
            self.dev = device

    # 이미지 로드 함수
    def img_load(self, img_path):
        source = str(img_path)
        is_file = Path(source).suffix[1:] in (IMG_FORMATS)
        if is_file == False:
            print("plz input only one image sorce")
            return None

        if is_file:
            source = check_file(source)  # download

        """origin_img = Image.open(path).convert('RGB')
        np_img = np.array(origin_img)
        img = Image.fromarray(np_img)
        self.img = img"""

    # 신경망 로드 함수
    def NN_load(self):
        model = torch.load(self.model_path)
        self.model = model

    def detect_object(self):
        pass