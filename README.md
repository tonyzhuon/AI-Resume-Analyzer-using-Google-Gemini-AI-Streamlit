# AI Resume Analyzer using Google Gemini AI & Streamlit

## 📌 Overview
The **AI Resume Analyzer** is a Streamlit-based web application that analyzes resumes (PDFs) and compares them with job descriptions using **Google Gemini AI**.  
It extracts text from resumes (supports both text-based and scanned PDFs using OCR) and provides:
- ✅ **Professional evaluation of the resume**
- ✅ **Skills already possessed by the candidate**
- ✅ **Skills & courses to improve the resume**
- ✅ **Strengths & weaknesses of the candidate**
- ✅ **Comparison with the provided Job Description**

---

## 🚀 Features
- **PDF Text Extraction:** Extracts text using `pdfplumber` and falls back to OCR (`pytesseract`) for scanned/image-based resumes.
- **AI-Powered Analysis:** Uses **Google Gemini 1.5 Flash** for professional HR-like evaluation.
- **Job Description Matching:** Highlights strengths/weaknesses based on the provided JD.
- **Interactive UI:** Built using **Streamlit** for easy usage.

---

## 🛠️ Tech Stack
- **Python 3.8+**
- **Streamlit**
- **Google Generative AI (Gemini 1.5 Flash)**
- **pdfplumber** (for text extraction)
- **pytesseract** (OCR for scanned PDFs)
- **pdf2image** (PDF to image conversion)

---


