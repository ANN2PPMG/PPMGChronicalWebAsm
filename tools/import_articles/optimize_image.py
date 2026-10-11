"""Resize and compress an image for the chronicle's local wwwroot/img archive.

Usage: python optimize_image.py <input> <output> [max_dim] [quality]
"""
import os
import sys

from PIL import Image


def optimize(input_path, output_path, max_dim=1600, quality=82):
    img = Image.open(input_path)
    if img.mode in ("RGBA", "P", "LA"):
        img = img.convert("RGB")

    width, height = img.size
    scale = min(1.0, max_dim / max(width, height))
    if scale < 1.0:
        img = img.resize((round(width * scale), round(height * scale)), Image.LANCZOS)

    os.makedirs(os.path.dirname(output_path) or ".", exist_ok=True)
    img.save(output_path, "JPEG", quality=quality, optimize=True)


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("Usage: python optimize_image.py <input> <output> [max_dim] [quality]")
        sys.exit(1)

    in_path, out_path = sys.argv[1], sys.argv[2]
    max_dim = int(sys.argv[3]) if len(sys.argv) > 3 else 1600
    quality = int(sys.argv[4]) if len(sys.argv) > 4 else 82

    optimize(in_path, out_path, max_dim, quality)
    before = os.path.getsize(in_path)
    after = os.path.getsize(out_path)
    print(f"Saved: {out_path} ({before // 1024} KB -> {after // 1024} KB)")
