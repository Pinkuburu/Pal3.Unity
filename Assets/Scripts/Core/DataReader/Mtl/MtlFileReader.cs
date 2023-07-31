﻿// ---------------------------------------------------------------------------------------------
//  Copyright (c) 2021-2023, Jiaqi Liu. All rights reserved.
//  See LICENSE file in the project root for license information.
// ---------------------------------------------------------------------------------------------

#define USE_UNSAFE_BINARY_READER

namespace Core.DataReader.Mtl
{
    using System.Collections.Generic;
    using System.IO;
    using Extensions;
    using GameBox;
    using Utils;

    public sealed class MtlFileReader : IFileReader<MtlFile>
    {
        private readonly int _codepage;

        public MtlFileReader(int codepage)
        {
            _codepage = codepage;
        }

        public MtlFile Read(byte[] data)
        {
            #if USE_UNSAFE_BINARY_READER
            using var reader = new UnsafeBinaryReader(data);
            #else
            using var stream = new MemoryStream(data);
            using var reader = new BinaryReader(stream);
            #endif

            var header = reader.ReadChars(4);
            var headerStr = new string(header[..^1]);

            if (headerStr != "mtl")
            {
                throw new InvalidDataException("Invalid MTL(.mtl) file: header != mtl");
            }

            var version = reader.ReadInt32();
            if (version != 100)
            {
                throw new InvalidDataException("Invalid MTL(.mtl) file: version != 100");
            }

            int numberOfMaterials = reader.ReadInt32();
            var materials = new GameBoxMaterial[numberOfMaterials];

            for (var i = 0; i < numberOfMaterials; i++)
            {
                materials[i] = new GameBoxMaterial
                {
                    Diffuse = Utility.ToColor(reader.ReadSingleArray(4)),
                    Ambient = Utility.ToColor(reader.ReadSingleArray(4)),
                    Specular = Utility.ToColor(reader.ReadSingleArray(4)),
                    Emissive = Utility.ToColor(reader.ReadSingleArray(4)),
                    SpecularPower = reader.ReadSingle(),
                };

                List<string> textureNames = new ();
                for (var j = 0; j < 4; j++)
                {
                    int textureNameLength = reader.ReadInt32();
                    if (textureNameLength == 0) continue;
                    textureNames.Add(reader.ReadString(textureNameLength, _codepage));
                }

                materials[i].TextureFileNames = textureNames.ToArray();
            }

            return new MtlFile(materials);
        }
    }
}