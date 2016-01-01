
import numpy as np
import  cv2
"""
global n
n = 0
img1 = cv2.imread('testimg.jpg', 0)
img2 = cv2.imread('testimg_diff.jpg', 0)
height, width = img1.shape
for line in range(height):
    for pixel in range(width):
        if img1[line][pixel] != img2[line][pixel]:
            n = n + 1

print n
"""
"""
import  time
from PIL import ImageGrab

start=time.clock()
im=ImageGrab.grab()
end=time.clock()
print (end-start)*1000,"ms"
"""
import cv2
global n
n = 0
img1 = cv2.imread("/Users/KS/Downloads/rec_3", 0)
img2 = cv2.imread("/Users/KS/Downloads/rec_4", 0)
height, width = img1.shape

for line in range(height):
    for pixel in range(width):
        if img1[line][pixel] != img2[line][pixel]:
            n = n + 1
print n