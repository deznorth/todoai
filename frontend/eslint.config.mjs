import { dirname } from 'path';
import { fileURLToPath } from 'url';
import { FlatCompat } from '@eslint/eslintrc';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const compat = new FlatCompat({
    baseDirectory: __dirname,
});

const eslintConfig = [
    ...compat.extends('next/core-web-vitals', 'next/typescript', 'prettier'),
    {
        rules: {
            // TypeScript strict rules
            '@typescript-eslint/no-explicit-any': 'error',
            '@typescript-eslint/no-unused-vars': 'error',
            '@typescript-eslint/no-non-null-assertion': 'warn',

            // React/Next.js specific rules
            'react/react-in-jsx-scope': 'off',
            'react-hooks/exhaustive-deps': 'warn',

            // General code quality
            'prefer-const': 'error',
            'no-var': 'error',
            'no-console': 'warn',
            eqeqeq: 'error',
        },
    },
];

export default eslintConfig;
