from PIL import Image, ImageDraw
import struct, os

# Generate icon directly to destination
dst = r'D:\Mario\RoknaCafe\src\RoknaCafe\rokn-hady.ico'
sizes = [16, 32, 48, 64, 128, 256]
images = []

for size in sizes:
    im = Image.new('RGBA', (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(im)
    
    if size <= 16:
        cx, cy = size // 2, size // 2
        r = size // 2 - max(1, size // 12)
        draw.ellipse([cx-r, cy-r, cx+r, cy+r], fill=(32, 42, 28))
        cup_w = int(r * 1.6)
        cup_h = int(r * 1.1)
        draw.rectangle([cx-cup_w//2, cy-cup_h//4, cx+cup_w//2, cy+cup_h*3//4], fill=(52, 152, 219))
    else:
        cx, cy = size // 2, size // 2
        r = size // 2 - max(1, size // 12)
        draw.ellipse([0, 0, size-1, size-1], fill=(32, 42, 28))
        
        cup_w = int(r * 1.55)
        cup_h = int(r * 1.08)
        cup_x = cx - cup_w // 2
        cup_y = cy + max(1, size // 12) * 6 // 10
        
        draw.rectangle([cup_x, cup_y, cup_x + cup_w, cup_y + cup_h], fill=(52, 152, 219))
        dark_h = int(cup_h * 0.34)
        draw.rectangle([cup_x, cup_y + cup_h - dark_h, cup_x + cup_w, cup_y + cup_h], fill=(41, 128, 185))
        
        handle_x = cup_x + cup_w
        handle_y = int(cup_y + cup_h * 0.22)
        handle_w = max(1, int(cup_h * 0.42))
        handle_h = max(1, int(cup_h * 0.56))
        draw.ellipse([handle_x, handle_y, handle_x + handle_w, handle_y + handle_h], fill=(235, 238, 242))
        
        if size >= 64:
            steam_y = cup_y - int(cup_h * 0.52)
            steam_color = (235, 238, 242, 220)
            draw.line([(cx - cup_w // 28, steam_y + size // 10), (cx - cup_w // 28 - size // 20, steam_y)], fill=steam_color, width=max(1, size // 56))
            draw.line([(cx, steam_y + size // 10), (cx + size // 100, steam_y)], fill=steam_color, width=max(1, size // 56))
            draw.line([(cx + cup_w // 28, steam_y + size // 10), (cx + cup_w // 28 + size // 20, steam_y)], fill=steam_color, width=max(1, size // 56))
    
    images.append(im)

images[0].save(dst, format='ICO', sizes=[(s, s) for s in sizes], append_images=images[1:])
print(f"Icon saved to: {dst}")
