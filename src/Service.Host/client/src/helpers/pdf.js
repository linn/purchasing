import html2canvas from 'html2canvas';
import { jsPDF as JsPDF } from 'jspdf';

export const savePdf = async ref => {
    const element = ref.current;
    const canvas = await html2canvas(element, {
        quality: 4,
        scale: 5
    });
    const data = canvas.toDataURL('image/png');

    const pdf = new JsPDF('p', 'pt', 'a4', true);
    const imgProperties = pdf.getImageProperties(data);
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = (imgProperties.height * pdfWidth) / imgProperties.width;

    pdf.addImage(data, 'PNG', 0, 0, pdfWidth, pdfHeight, '', 'FAST');
    pdf.save();
};

export const emailPdf = async (ref, sendEmailCallback) => {
    const element = ref.current;
    const canvas = await html2canvas(element, {
        quality: 4,
        scale: 5
    });
    const data = canvas.toDataURL('image/png');

    const pdf = new JsPDF('p', 'pt', 'a4', true);
    const imgProperties = pdf.getImageProperties(data);
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = (imgProperties.height * pdfWidth) / imgProperties.width;

    pdf.addImage(data, 'PNG', 0, 0, pdfWidth, pdfHeight, '', 'FAST');

    const blob = pdf.output('blob');
    sendEmailCallback(blob);
};
